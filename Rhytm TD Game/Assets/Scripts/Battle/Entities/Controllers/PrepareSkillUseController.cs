using System.Collections.Generic;
using CoreFramework;
using CoreFramework.Rhytm;
using RhytmTD.Battle.Entities.Models;
using RhytmTD.Core;
using RhytmTD.Data.Models;

namespace RhytmTD.Battle.Entities.Controllers
{
    /// <summary>
    /// Is responsible for using ability after it was selected in spellbook
    /// </summary>
    public class PrepareSkillUseController : BaseController
    {
        private UpdateModel m_UpdateModel;
        private BattleModel m_BattleModel;
        private PrepareSkilIUseModel m_PrepareSkilIUseModel;

        private RhytmController m_RhytmController;
        private SkillsController m_SkillsController;
        private FindTargetController m_FindTargetController;

        private AnimationModule m_PlayerAnimationModule;
        private LoadoutModule m_PlayerLodoutModule;
        private BattleEntity m_TargetEntity;
        private int m_SkillID;

        private bool m_IsSequenceStrted;
        private Dictionary<int, bool[]> m_SkillTypeID2Pattern;
        private Dictionary<int, SkillProgress> m_SkillTypeID2Progress;


        public PrepareSkillUseController(Dispatcher dispatcher) : base(dispatcher)
        {
        }
        
        public override void InitializeComplete()
        {
            base.InitializeComplete();

            m_UpdateModel = Dispatcher.GetModel<UpdateModel>();

            m_BattleModel = Dispatcher.GetModel<BattleModel>();
            m_BattleModel.OnPlayerEntityInitialized += PlayerInitializedHandlder;
            m_BattleModel.OnSpellbookOpened += SpellbookOpenedHandler;
            m_BattleModel.OnSpellbookClosed += SpellbookClosedHandler;
            m_BattleModel.OnSpellbookUsed += SpellbookClosedHandler;

            m_PrepareSkilIUseModel = Dispatcher.GetModel<PrepareSkilIUseModel>();
            m_PrepareSkilIUseModel.OnCorrectTouch += CorrectTouchHandler;
            m_PrepareSkilIUseModel.OnWrongTouch += WrongTouchHandler;

            m_RhytmController = Dispatcher.GetController<RhytmController>();
            m_SkillsController = Dispatcher.GetController<SkillsController>();
            m_FindTargetController = Dispatcher.GetController<FindTargetController>();
        }


        private void SpellbookOpenedHandler()
        {
            m_UpdateModel.OnUpdate += UpdateHandler;
        }

        private void SpellbookClosedHandler()
        {
            m_UpdateModel.OnUpdate -= UpdateHandler;
        }

        private void UpdateHandler(float deltaTime)
        {
            
        }


        private void CorrectTouchHandler()
        {
            UnityEngine.Debug.LogError("TOUCH at tick " + m_RhytmController.CurrentTick);

            if (!m_IsSequenceStrted)
            {
                HandleStartSequence();
            }
            else
            {
                HandleContinueSequence();
            }
        }

        private void WrongTouchHandler()
        {
            foreach (int skillID in m_SkillTypeID2Pattern.Keys)
            {
                ResetSkillProgress(skillID);
            }

            HandleAllSkillsReset();
        }

        private void EventProcessingTickHandler(int ticksSinceStart)
        {
            foreach (int skillID in m_SkillTypeID2Pattern.Keys)
            {
                //If current tick is next tick for the skill - skill is missed
                if (m_SkillTypeID2Progress[skillID].NextTick == ticksSinceStart)
                {
                    UnityEngine.Debug.Log("Auto reset for " + skillID);
                    ResetSkillProgress(skillID);
                }
                else
                {
                    m_PrepareSkilIUseModel.OnSpellNextTickAuto(skillID);
                }
            }

            HandleAllSkillsReset();
        }


        private void HandleStartSequence()
        {
            m_IsSequenceStrted = true;

            //Set initial value for all skills
            foreach (int skillID in m_SkillTypeID2Pattern.Keys)
            {
                m_SkillTypeID2Progress[skillID].NextTick = m_RhytmController.CurrentTick;
                CalculateNextTickForSkill(skillID);
            }

            //Subscribe for processing tick event
            m_RhytmController.OnEventProcessingTick += EventProcessingTickHandler;
        }

        private void HandleContinueSequence()
        {
            foreach (int skillID in m_SkillTypeID2Pattern.Keys)
            {
                //If current tick is correct for the skill
                if (m_RhytmController.CurrentTick == m_SkillTypeID2Progress[skillID].NextTick)
                {
                    CalculateNextTickForSkill(skillID);
                }

                else
                {
                    UnityEngine.Debug.LogError("Wrong for " + skillID);
                    ResetSkillProgress(skillID);
                }
            }

            HandleAllSkillsReset();
        }

        private void HandleAllSkillsReset()
        {
            if (IsAllSkillsReset())
            {
                UnityEngine.Debug.LogError("All reseted");

                m_RhytmController.OnEventProcessingTick -= EventProcessingTickHandler;
                m_IsSequenceStrted = false;

                m_PrepareSkilIUseModel.OnAllSpellsReset?.Invoke();
            }
        }

        private void HandleSkillSelection(int skillTypeID)
        {
            UnityEngine.Debug.Log("Cast selected " + skillTypeID);

            m_PrepareSkilIUseModel.OnSpellSelected?.Invoke(skillTypeID);
            m_BattleModel.OnSpellbookUsed?.Invoke();

            StartUseSkill(skillTypeID, m_PlayerLodoutModule.GetSkillIDByTypeID(skillTypeID));
        }


        private void CalculateNextTickForSkill(int skillTypeID)
        {
            SkillProgress skillProgress = m_SkillTypeID2Progress[skillTypeID];
            bool[] skillPattern = m_SkillTypeID2Pattern[skillTypeID];

            for (int i = skillProgress.CurProgressIndex; i < skillPattern.Length; i++)
            {
                skillProgress.NextTick++;
                skillProgress.CurProgressIndex++;

                if (skillProgress.CurProgressIndex < skillPattern.Length && skillPattern[skillProgress.CurProgressIndex])
                {
                    break;
                }
            }

            if (m_SkillTypeID2Progress[skillTypeID].CurProgressIndex >= skillPattern.Length)
            {
                HandleSkillSelection(skillTypeID);
            }
            else
            {
                UnityEngine.Debug.Log("Next tick at: " + m_SkillTypeID2Progress[skillTypeID].NextTick + " for " + skillTypeID);
                m_PrepareSkilIUseModel.OnSpellNextTickInput(skillTypeID);
            }
        }

        private void ResetSkillProgress(int skillTypeID)
        {
            m_SkillTypeID2Progress[skillTypeID].Reset();
            m_PrepareSkilIUseModel.OnSpellReset?.Invoke(skillTypeID);
        }
         
        private bool IsAllSkillsReset()
        {
            int amountOfResetedSkills = 0;
            foreach (int skillID in m_SkillTypeID2Pattern.Keys)
            {
                if (m_SkillTypeID2Progress[skillID].IsReset)
                {
                    amountOfResetedSkills++;
                }
            }

            return amountOfResetedSkills == m_SkillTypeID2Pattern.Count;
        }


        private void StartUseSkill(int skillTypeID, int skillID)
        {
            m_SkillID = skillID;
            m_TargetEntity = GetTargetBySkillType(skillTypeID);

            if (m_TargetEntity != null)
            {
                m_PlayerAnimationModule.OnAnimationMoment += SkillAnimationMomentHandler;
                m_PlayerAnimationModule.PlayAnimation(ConvertersCollection.ConvertSkillTypeID2AnimationType(skillTypeID));
            }
        }

        private void SkillAnimationMomentHandler()
        {
            m_PlayerAnimationModule.OnAnimationMoment -= SkillAnimationMomentHandler;
            m_SkillsController.UseSkill(m_SkillID, m_BattleModel.PlayerEntity.ID, m_TargetEntity.ID);

            m_BattleModel.OnSpellbookPostUsed?.Invoke();
        }

        private void PlayerInitializedHandlder(BattleEntity playerEntity)
        {
            m_BattleModel.OnPlayerEntityInitialized -= PlayerInitializedHandlder;

            m_PlayerAnimationModule = playerEntity.GetModule<AnimationModule>();
            m_PlayerLodoutModule = playerEntity.GetModule<LoadoutModule>();

            InitializePatterns();
        }

        private void InitializePatterns()
        {
            m_SkillTypeID2Pattern = new Dictionary<int, bool[]>();
            m_SkillTypeID2Progress = new Dictionary<int, SkillProgress>();

            foreach (int skillTypeID in m_PlayerLodoutModule.SelectedSkillTypeIDs)
            {
                //Fill patterns
                m_SkillTypeID2Pattern.Add(skillTypeID, tempSkillPatterns[skillTypeID]);

                //Fill progress by init values
                m_SkillTypeID2Progress.Add(skillTypeID, new SkillProgress());
            }
        }

        private BattleEntity GetTargetBySkillType(int skillTypeID)
        {
            BattleEntity target = null;
            switch (skillTypeID)
            {
                case ConstsCollection.SkillConsts.FIREBALL_ID:
                case ConstsCollection.SkillConsts.METEORITE_ID:
                    target = m_FindTargetController.GetNearestTarget(m_BattleModel.PlayerEntity);
                    break;
                case ConstsCollection.SkillConsts.HEALTH_ID:
                    target = m_BattleModel.PlayerEntity;
                    break;
            }

            return target;
        }


        private class SkillProgress
        {
            public int CurProgressIndex;
            public int NextTick;

            public bool IsReset => NextTick == 0;

            public void Reset() => CurProgressIndex = NextTick = 0;
        }


        public static Dictionary<int, bool[]> tempSkillPatterns = new Dictionary<int, bool[]>()
        {
            { ConstsCollection.SkillConsts.FIREBALL_ID, new bool[]  { true, true, false, true,  true } },
            { ConstsCollection.SkillConsts.METEORITE_ID, new bool[] { true, true, false, false, true } },
            { ConstsCollection.SkillConsts.HEALTH_ID, new bool[]    { true, true, false, true,  false, true } },
        };
    }
}
