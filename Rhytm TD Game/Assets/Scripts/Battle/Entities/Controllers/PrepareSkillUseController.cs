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
        private SkillsModel m_SkillsModel;
        private PrepareSkilIUseModel m_PrepareSkilIUseModel;

        private RhytmController m_RhytmController;
        private SkillsController m_SkillsController;
        private FindTargetController m_FindTargetController;

        private AnimationModule m_PlayerAnimationModule;
        private LoadoutModule m_PlayerLodoutModule;
        private BattleEntity m_TargetEntity;
        private int m_SkillID;

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

            m_SkillsModel = Dispatcher.GetModel<SkillsModel>();
            m_SkillsModel.OnPrepareSkill += PrepareSkillHandler;

            m_PrepareSkilIUseModel = Dispatcher.GetModel<PrepareSkilIUseModel>();
            m_PrepareSkilIUseModel.OnTouch += TouchHandler;

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
            UnityEngine.Debug.Log("Update " + m_RhytmController.ProgressToNextTickAnalog);
        }

        private void TouchHandler()
        {
            UnityEngine.Debug.Log("TOUCH");
        }

        private void PrepareSkillHandler(int skillTypeID, int skillID)
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
    }
}
