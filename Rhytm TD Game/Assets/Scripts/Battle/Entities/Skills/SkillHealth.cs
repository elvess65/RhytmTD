
using RhytmTD.Battle.Entities.Controllers;
using RhytmTD.Battle.Entities.Models;
using UnityEngine;
using static CoreFramework.EnumsCollection;

namespace RhytmTD.Battle.Entities.Skills
{
    public class SkillHealth : BaseSkill
    {
        private BattleModel m_BattleModel;
        private EffectsController m_EffectController;
        private MarkerController m_MarkerController;

        private HealthSkillModule m_HealthSkillModule;
        private int m_MarkerID;

        public override void Initialize(BattleEntity battleEntity)
        {
            base.Initialize(battleEntity);

            m_HealthSkillModule = battleEntity.GetModule<HealthSkillModule>();

            m_BattleModel = Dispatcher.GetModel<BattleModel>();
            m_EffectController = Dispatcher.GetController<EffectsController>();
            m_MarkerController = Dispatcher.GetController<MarkerController>();
        }

        public override void UseSkill(int senderID)
        {
            base.UseSkill(senderID);

            BattleEntity sender = m_BattleModel.GetEntity(senderID);
            TargetModule targetModule = sender.GetModule<TargetModule>();

            if (targetModule.HasTarget)
            {
                m_MarkerID = m_MarkerController.ShowTargetFollowingMarker(MarkerTypes.AllyTarget, targetModule.Target);
            }
        }

        protected override void SkilUseStarted(int senderID)
        {
            base.SkilUseStarted(senderID);

            BattleEntity sender = m_BattleModel.GetEntity(senderID);
            TargetModule targetModule = sender.GetModule<TargetModule>();

            if (targetModule.HasTarget)
            {
                TransformModule targerTransform = targetModule.Target.GetModule<TransformModule>();
                HealthModule healthModule = targetModule.Target.GetModule<HealthModule>();

                int healthToRestore = Mathf.Min(Mathf.RoundToInt(healthModule.Health * m_HealthSkillModule.HealthPercent), healthModule.Health - healthModule.CurrentHealth);
                healthModule.AddHealth(healthToRestore);

                m_EffectController.CreateHealthEffect(targerTransform.Position, targerTransform.Rotation);
            }
        }

        protected override void SkillUseFinished(int senderID)
        {
            base.SkillUseFinished(senderID);

            m_MarkerController.HideMarker(m_MarkerID);
        }
    }
}
