
using RhytmTD.Battle.Entities.Controllers;
using RhytmTD.Battle.Entities.Models;
using RhytmTD.Data;
using UnityEngine;

namespace RhytmTD.Battle.Entities.Skills
{
    public class SkillMeteorite : BaseSkill
    {
        private BattleModel m_BattleModel;
        private DamageController m_DamageController;
        private EffectsController m_EffectController;

        private MeteoriteModule m_MeteoriteModule;

        public override void Initialize(BattleEntity battleEntity)
        {
            base.Initialize(battleEntity);

            m_MeteoriteModule = battleEntity.GetModule<MeteoriteModule>();

            m_BattleModel = Dispatcher.GetModel<BattleModel>();
            m_DamageController = Dispatcher.GetController<DamageController>();
            m_EffectController = Dispatcher.GetController<EffectsController>();
        }

        protected async override void SkilUseStarted(int senderID, int targetID)
        {
            base.SkilUseStarted(senderID, targetID);

            BattleEntity target = m_BattleModel.GetEntity(targetID);
            TransformModule targetTransform = target.GetModule<TransformModule>();

            BattleEntity effectEntity = m_EffectController.CreateMeteoriteEffect();
            EffectModule effectModule = effectEntity.GetModule<EffectModule>();

            EffectCreated(effectModule, targetTransform.Position);

            await new WaitForSeconds(m_MeteoriteModule.FlyTime);

            BlowMeteorite(effectModule);

            foreach (BattleEntity battleEntity in m_BattleModel.BattleEntities)
            {
                if (!battleEntity.HasModule<EnemyBehaviourTag>())
                    continue;

                DestroyModule destroyModule = battleEntity.GetModule<DestroyModule>();
                if (!destroyModule.IsDestroyed)
                {
                    TransformModule transformModule = battleEntity.GetModule<TransformModule>();
                    float disntanceSqr = (targetTransform.Position - transformModule.Position).sqrMagnitude;

                    if (disntanceSqr <= m_MeteoriteModule.DamageRadius * m_MeteoriteModule.DamageRadius)
                    {
                        m_DamageController.DealDamage(senderID, battleEntity.ID, m_MeteoriteModule.Damage);
                    }
                }
            }
        }

        private void EffectCreated(EffectModule effectModule, Vector3 targetPosition)
        {
            DataContainer data = new DataContainer();
            data.AddString("action", "create");
            data.AddVector("position", targetPosition);

            effectModule.EffectAction(data);
        }

        private void BlowMeteorite(EffectModule effectModule)
        {
            DataContainer data = new DataContainer();
            data.AddString("action", "blow");

            effectModule.EffectAction(data);
        }
    }
}
