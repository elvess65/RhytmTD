
using RhytmTD.Battle.Entities.Controllers;
using RhytmTD.Battle.Entities.Models;
using UnityEngine;

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

        public override void UseSkill(int senderID, int targetID)
        {
            base.UseSkill(senderID, targetID);

            BattleEntity target = m_BattleModel.GetEntity(targetID);

            m_MarkerID = m_MarkerController.ShowTargetMarker(MarkerTypes.AllyTarget, target);
        }

        protected override void SkilUseStarted(int senderID, int targetID)
        {
            base.SkilUseStarted(senderID, targetID);

            BattleEntity target = m_BattleModel.GetEntity(targetID);
            TransformModule targerTransform = target.GetModule<TransformModule>();

            HealthModule healthModule = target.GetModule<HealthModule>();

            int healthToRestore = Mathf.Min(Mathf.RoundToInt(healthModule.Health * m_HealthSkillModule.HealthPercent), healthModule.Health - healthModule.CurrentHealth);
            healthModule.AddHealth(healthToRestore);

            m_EffectController.CreateHealthEffect(targerTransform.Position, targerTransform.Rotation);

            m_MarkerController.HideMarker(m_MarkerID);
        }
    }
}
