
using CoreFramework;
using CoreFramework.Input;
using CoreFramework.Rhytm;
using RhytmTD.Battle.Entities.Controllers;
using RhytmTD.Battle.Entities.Models;
using RhytmTD.Data;
using UnityEngine;

namespace RhytmTD.Battle.Entities.Skills
{
    public class SkillFireball : BaseSkill
    {
        private const float m_DEFAULT_FIREBALL_MOVE_TIME = 10;
        private const float m_DEFAULT_FIREBALL_SPEED = 5;

        private BattleModel m_BattleModel;
        private InputModel m_InputModel;
        private DamageController m_DamageController;
        private EffectsController m_EffectController;
        private RhytmController m_RhytmController;

        private FireballSkillModule m_FireballModule;

        public override void Initialize(BattleEntity battleEntity)
        {
            base.Initialize(battleEntity);

            m_FireballModule = battleEntity.GetModule<FireballSkillModule>();

            m_BattleModel = Dispatcher.GetModel<BattleModel>();
            m_InputModel = Dispatcher.GetModel<InputModel>();

            m_DamageController = Dispatcher.GetController<DamageController>();
            m_EffectController = Dispatcher.GetController<EffectsController>();
            m_RhytmController = Dispatcher.GetController<RhytmController>();
        }

        protected async override void SkilUseStarted(int senderID)
        {
            base.SkilUseStarted(senderID);

            BattleEntity sender = m_BattleModel.GetEntity(senderID);

            TargetModule targetModule = sender.GetModule<TargetModule>();
            SlotModule senderSlot = sender.GetModule<SlotModule>();

            Vector3 shootPosition = senderSlot.ProjectileSlot.transform.position;

            Vector3 targetDirectionNormalized;
            float moveTime = m_DEFAULT_FIREBALL_MOVE_TIME;
            float fireballSpeed = m_DEFAULT_FIREBALL_SPEED;
            BattleEntity target = targetModule.Target;

            if (targetModule.HasTarget)
            {
                TransformModule targetTransform = targetModule.Target.GetModule<TransformModule>();
                Vector3 targetDirection = targetTransform.Position - shootPosition;

                float targetDistance = targetDirection.magnitude;
                targetDirectionNormalized = targetDirection / targetDistance;

                moveTime = m_RhytmController.GetTimeToNextTick();
                fireballSpeed = targetDistance / moveTime;
            }
            else
            {
                targetDirectionNormalized = (m_InputModel.LastTouchHitPoint - shootPosition).normalized;
                targetDirectionNormalized.y = 0;
            }

            Quaternion fireballRotation = Quaternion.LookRotation(targetDirectionNormalized);

            BattleEntity battleEntitiy = m_EffectController.CreateFireballEffect(senderSlot.ProjectileSlot.position, fireballRotation, fireballSpeed);
            EffectModule effectModule = battleEntitiy.GetModule<EffectModule>();
            
            if (targetModule.HasTarget && sender.HasModule<DamagePredictionModule>())
            {
                DamagePredictionModule damagePredictionModule = sender.GetModule<DamagePredictionModule>();
                damagePredictionModule.PotentialDamage += m_FireballModule.Damage;
            }

            MoveModule fireballMoveModule = battleEntitiy.GetModule<MoveModule>();
            fireballMoveModule.StartMove(targetDirectionNormalized);

            m_BattleModel.AddBattleEntity(battleEntitiy);

            await new WaitForSeconds(moveTime);

            fireballMoveModule.Stop();

            if (target != null)
            {
                m_DamageController.DealDamage(senderID, target.ID, m_FireballModule.Damage);
            }

            BlowFireball(effectModule);

            m_BattleModel.RemoveBattleEntity(battleEntitiy.ID);
        }

        private void BlowFireball(EffectModule effectModule)
        {
            DataContainer data = new DataContainer();
            data.AddObject(ConstsCollection.DataConsts.ACTION, ConstsCollection.DataConsts.EXPLOSION);

            effectModule.EffectAction(data);
        }
    }
}
