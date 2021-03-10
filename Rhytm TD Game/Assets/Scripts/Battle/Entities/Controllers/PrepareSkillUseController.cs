using System.Collections.Generic;
using CoreFramework;
using CoreFramework.Rhytm;
using RhytmTD.Battle.Entities.Models;
using RhytmTD.Core;
using RhytmTD.Data.Models.DataTableModels;
using UnityEngine;

namespace RhytmTD.Battle.Entities.Controllers
{
    /// <summary>
    /// Is responsible for using ability after it was selected in spellbook
    /// </summary>
    public class PrepareSkillUseController : BaseController
    {
        private BattleModel m_BattleModel;
        private PrepareSkilIUseModel m_PrepareSkilIUseModel;
        private AccountBaseParamsDataModel m_AccountBaseParamsDataModel;

        private RhytmController m_RhytmController;
        private SkillsController m_SkillsController;
        private FindTargetController m_FindTargetController;

        private AnimationModule m_PlayerAnimationModule;
        private LoadoutModule m_PlayerLodoutModule;
        private BattleEntity m_TargetEntity;
        private int m_SkillID;
        private int m_SkillTypeID;

        private bool m_IsSequenceStrted;
        private Dictionary<int, bool[]> m_SkillTypeID2Pattern;
        private Dictionary<int, SkillProgress> m_SkillTypeID2Progress;


        public PrepareSkillUseController(Dispatcher dispatcher) : base(dispatcher)
        {
        }
        
        public override void InitializeComplete()
        {
            base.InitializeComplete();

            m_BattleModel = Dispatcher.GetModel<BattleModel>();
            m_BattleModel.OnPlayerEntityInitialized += PlayerInitializedHandlder;
            m_BattleModel.OnSpellbookOpened += SpellbookOpenedHandler;
            m_BattleModel.OnSpellbookClosed += SpellbookClosedOrUsedHandler;
            m_BattleModel.OnSpellbookUsed += SpellbookClosedOrUsedHandler;

            m_PrepareSkilIUseModel = Dispatcher.GetModel<PrepareSkilIUseModel>();
            m_PrepareSkilIUseModel.OnCorrectTouch += CorrectTouchHandler;
            m_PrepareSkilIUseModel.OnWrongTouch += WrongTouchHandler;
            m_PrepareSkilIUseModel.OnSkilDirectionSelected += SkillDirectionSelectedeHandler;

            m_AccountBaseParamsDataModel = Dispatcher.GetModel<AccountBaseParamsDataModel>();

            m_RhytmController = Dispatcher.GetController<RhytmController>();
            m_SkillsController = Dispatcher.GetController<SkillsController>();
            m_FindTargetController = Dispatcher.GetController<FindTargetController>();
        }


        private void SpellbookOpenedHandler()
        {
        }

        private void SpellbookClosedOrUsedHandler()
        {
        }

        private void PlayerInitializedHandlder(BattleEntity playerEntity)
        {
            m_BattleModel.OnPlayerEntityInitialized -= PlayerInitializedHandlder;

            m_PlayerAnimationModule = playerEntity.GetModule<AnimationModule>();
            m_PlayerLodoutModule = playerEntity.GetModule<LoadoutModule>();

            InitializePatterns();
        }

        #region Sequence

        private void CorrectTouchHandler()
        {
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

            HandleSequenceFailed();
        }

        private void SkillDirectionSelectedeHandler(Vector3 dir)
        {
            Debug.Log(dir + " " + m_SkillTypeID);

            StartUseSkill(m_SkillTypeID, m_PlayerLodoutModule.GetSkillIDByTypeID(m_SkillTypeID));
        }

        private void EventProcessingTickHandler(int ticksSinceStart)
        {
            foreach (int skillID in m_SkillTypeID2Pattern.Keys)
            {
                //If current tick is next tick for the skill - skill is missed
                if (m_SkillTypeID2Progress[skillID].NextTick == ticksSinceStart)
                {
                    ResetSkillProgress(skillID);
                }
                else
                {
                    m_PrepareSkilIUseModel.OnSkillStepReachedAuto(skillID);
                }
            }

            HandleSequenceFailed();
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
                    ResetSkillProgress(skillID);
                }
            }

            HandleSequenceFailed();
        }

        private void HandleSequenceFailed()
        {
            if (IsAllSkillsReseted())
            {
                m_RhytmController.OnEventProcessingTick -= EventProcessingTickHandler;
                m_IsSequenceStrted = false;

                m_PrepareSkilIUseModel.OnSequenceFailed?.Invoke();
            }
        }

        private void HandleSkillSelection(int skillTypeID)
        {
            m_SkillTypeID = skillTypeID;

            m_PrepareSkilIUseModel.OnSkillSelected?.Invoke(skillTypeID);

            switch(m_AccountBaseParamsDataModel.GetSkillBaseDataByID(skillTypeID).TargetingType)
            {
                case EnumsCollection.SkillTargetingType.Area:
                case EnumsCollection.SkillTargetingType.Direction:
                    m_BattleModel.OnDirectionalSpellSelected?.Invoke();
                    break;
                case EnumsCollection.SkillTargetingType.Self:
                    StartUseSkill(m_SkillTypeID, m_PlayerLodoutModule.GetSkillIDByTypeID(m_SkillTypeID));
                    break;
            } 
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
                m_PrepareSkilIUseModel.OnSkillStepReachedInput(skillTypeID);
            }
        }

        private void ResetSkillProgress(int skillTypeID)
        {
            m_SkillTypeID2Progress[skillTypeID].Reset();
            m_PrepareSkilIUseModel.OnSkillReset?.Invoke(skillTypeID);
        }
         
        private bool IsAllSkillsReseted()
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

        #endregion

        private void StartUseSkill(int skillTypeID, int skillID)
        {
            m_BattleModel.OnSpellbookUsed?.Invoke();

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