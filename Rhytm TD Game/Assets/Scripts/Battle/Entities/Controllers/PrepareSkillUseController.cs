using CoreFramework;
using RhytmTD.Battle.Entities.Models;
using UnityEngine;

namespace RhytmTD.Battle.Entities.Controllers
{
    public class PrepareSkillUseController : BaseController
    {
        public PrepareSkillUseController(Dispatcher dispatcher) : base(dispatcher)
        {
        }

        private BattleModel m_BattleModel;
        private FindTargetController m_FindTargetController;
        private SkillsController m_SkillsController;

        private AnimationModule m_PlayerAnimationModule;
        private BattleEntity m_TargetEntity;
        private int m_SkillID;

        public override void InitializeComplete()
        {
            base.InitializeComplete();

            m_BattleModel = Dispatcher.GetModel<BattleModel>();
            m_BattleModel.OnPlayerEntityInitialized += PlayerInitializedHandlder;
            m_FindTargetController = Dispatcher.GetController<FindTargetController>();
            m_SkillsController = Dispatcher.GetController<SkillsController>();
        }

        public void StartUseSkill(int skillID)
        {
            Debug.Log("Prepare skill use " + skillID);
            m_SkillID = skillID;
            m_TargetEntity = m_FindTargetController.GetNearestTarget(m_BattleModel.PlayerEntity);
            if (m_TargetEntity != null)
            {
                LoadoutModule loadoutModule = m_BattleModel.PlayerEntity.GetModule<LoadoutModule>();
                //m_SkillID = loadoutModule.GetSkillIDByTypeID(skillID);

                m_PlayerAnimationModule.OnAnimationMoment += SkillAnimationMomentHandler;
                m_PlayerAnimationModule.PlayAnimation(EnumsCollection.AnimationTypes.UseWeaponSkill);
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
