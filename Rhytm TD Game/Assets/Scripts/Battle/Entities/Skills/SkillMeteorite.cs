
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
        private MarkerController m_MarkerController;

        private MeteoriteSkillModule m_MeteoriteModule;
        private int m_MarkerID;

        public override void Initialize(BattleEntity battleEntity)
        {
            base.Initialize(battleEntity);

            m_MeteoriteModule = battleEntity.GetModule<MeteoriteSkillModule>();

            m_BattleModel = Dispatcher.GetModel<BattleModel>();
            m_DamageController = Dispatcher.GetController<DamageController>();
            m_EffectController = Dispatcher.GetController<EffectsController>();
            m_MarkerController = Dispatcher.GetController<MarkerController>();
        }

        public override void UseSkill(int senderID, int targetID)
        {
            base.UseSkill(senderID, targetID);

            BattleEntity target = m_BattleModel.GetEntity(targetID);

            m_MarkerID = m_MarkerController.ShowAttackRadiusMarker(MarkerTypes.AttackRadius, target, m_MeteoriteModule.DamageRadius);
        }

        protected async override void SkilUseStarted(int senderID, int targetID)
        {
            base.SkilUseStarted(senderID, targetID);

            BattleEntity sender = m_BattleModel.GetEntity(senderID);
            BattleEntity target = m_BattleModel.GetEntity(targetID);

            TransformModule senderTransform = sender.GetModule<TransformModule>();
            TransformModule targetTransform = target.GetModule<TransformModule>();

            Vector3 effectPosition = senderTransform.Position + m_MeteoriteModule.EffectOffset;
            Vector3 targetDirection = targetTransform.Position - effectPosition;
    
            float effectMoveSpeed = targetDirection.magnitude / m_MeteoriteModule.FlyTime;
            BattleEntity effectEntity = m_EffectController.CreateMeteoriteEffect(effectPosition, Quaternion.identity, effectMoveSpeed);
            EffectModule effectModule = effectEntity.GetModule<EffectModule>();
            MoveModule effectMoveModule = effectEntity.GetModule<MoveModule>();

            effectMoveModule.StartMove(targetDirection.normalized);

            m_BattleModel.AddBattleEntity(effectEntity);

            await new WaitForSeconds(m_MeteoriteModule.FlyTime);

            effectMoveModule.Stop();

            m_MarkerController.HideMarker(m_MarkerID);

            BlowMeteorite(effectModule, m_MeteoriteModule.DamageRadius);

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

            m_BattleModel.RemoveBattleEntity(effectEntity.ID);
        }

        private void BlowMeteorite(EffectModule effectModule, float radius)
        {
            DataContainer data = new DataContainer();
            data.AddString(DataConsts.ACTION, DataConsts.BLOW);
            data.AddFloat(DataConsts.RADIUS, radius);

            effectModule.EffectAction(data);
        }
    }
}
