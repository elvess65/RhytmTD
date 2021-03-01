using CoreFramework;
using CoreFramework.Rhytm;
using RhytmTD.Battle.Entities.Models;
using RhytmTD.Core;
using UnityEngine;

namespace RhytmTD.Battle.Entities.Controllers
{
    public class PrepareSkillUseController : BaseController
    {
        private BattleModel m_BattleModel;
        private SkillsModel m_SkillsModel;
        private RhytmController m_RhytmController;
        private SkillsController m_SkillsController;
        private FindTargetController m_FindTargetController;

        private AnimationModule m_PlayerAnimationModule;
        private BattleEntity m_TargetEntity;
        private int m_SkillID;

        public PrepareSkillUseController(Dispatcher dispatcher) : base(dispatcher)
        {
        }
        
        public override void InitializeComplete()
        {
            base.InitializeComplete();

            m_BattleModel = Dispatcher.GetModel<BattleModel>();
            m_BattleModel.OnPlayerEntityInitialized += PlayerInitializedHandlder;

            m_SkillsModel = Dispatcher.GetModel<SkillsModel>();
            m_SkillsModel.OnPrepareSkill += PrepareSkillHandler;

            m_RhytmController = Dispatcher.GetController<RhytmController>();
            m_SkillsController = Dispatcher.GetController<SkillsController>();
            m_FindTargetController = Dispatcher.GetController<FindTargetController>();
        }

        private void PrepareSkillHandler(int skillTypeID, int skillID)
        {
            m_SkillID = skillID;
            m_TargetEntity = m_FindTargetController.GetNearestTarget(m_BattleModel.PlayerEntity);

            if (m_TargetEntity != null)
            {
                LoadoutModule loadoutModule = m_BattleModel.PlayerEntity.GetModule<LoadoutModule>();

                m_PlayerAnimationModule.OnAnimationMoment += SkillAnimationMomentHandler;
                m_PlayerAnimationModule.PlayAnimation(ConvertersCollection.ConvertSkillTypeID2AnimationType(skillTypeID));
            }
        }

        private void SkillAnimationMomentHandler()
        {
            m_PlayerAnimationModule.OnAnimationMoment -= SkillAnimationMomentHandler;
            m_SkillsController.UseSkill(m_SkillID, m_BattleModel.PlayerEntity.ID, m_TargetEntity.ID);
        }

        private void PlayerInitializedHandlder(BattleEntity playerEntity)
        {
            m_BattleModel.OnPlayerEntityInitialized -= PlayerInitializedHandlder;

            m_PlayerAnimationModule = playerEntity.GetModule<AnimationModule>();
        }
    }
}
