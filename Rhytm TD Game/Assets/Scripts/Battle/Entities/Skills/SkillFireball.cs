﻿
using CoreFramework;
using CoreFramework.Rhytm;
using RhytmTD.Battle.Entities.Controllers;
using RhytmTD.Battle.Entities.Models;
using RhytmTD.Data;
using UnityEngine;

namespace RhytmTD.Battle.Entities.Skills
{
    public class SkillFireball : BaseSkill
    {
        private BattleModel m_BattleModel;
        private DamageController m_DamageController;
        private EffectsController m_EffectController;
        private RhytmController m_RhytmController;

        private FireballSkillModule m_FireballModule;

        public override void Initialize(BattleEntity battleEntity)
        {
            base.Initialize(battleEntity);

            m_FireballModule = battleEntity.GetModule<FireballSkillModule>();

            m_BattleModel = Dispatcher.GetModel<BattleModel>();
            m_DamageController = Dispatcher.GetController<DamageController>();
            m_EffectController = Dispatcher.GetController<EffectsController>();
            m_RhytmController = Dispatcher.GetController<RhytmController>();
        }

        protected async override void SkilUseStarted(int senderID, int targetID)
        {
            base.SkilUseStarted(senderID, targetID);

            BattleEntity sender = m_BattleModel.GetEntity(senderID);
            BattleEntity target = m_BattleModel.GetEntity(targetID);

            TransformModule targetTransform = target.GetModule<TransformModule>();

            SlotModule senderSlot = sender.GetModule<SlotModule>();
            Vector3 shootPosition = senderSlot.ProjectileSlot.transform.position;

            Vector3 targetDirection = targetTransform.Position - shootPosition;
            float targetDistance = targetDirection.magnitude;
            Vector3 targetDirectionNormalized = targetDirection / targetDistance;

            Quaternion fireballRotation = Quaternion.LookRotation(targetDirectionNormalized);
            float moveTime = GetTimeToNextTick();
            float fireballSpeed = targetDistance / moveTime;

            BattleEntity battleEntitiy = m_EffectController.CreateFireballEffect(senderSlot.ProjectileSlot.position, fireballRotation, fireballSpeed);
            EffectModule effectModule = battleEntitiy.GetModule<EffectModule>();
            
            if (sender.HasModule<DamagePredictionModule>())
            {
                DamagePredictionModule damagePredictionModule = sender.GetModule<DamagePredictionModule>();
                damagePredictionModule.PotentialDamage += m_FireballModule.Damage;
            }

            MoveModule fireballMoveModule = battleEntitiy.GetModule<MoveModule>();
            fireballMoveModule.StartMove(targetDirectionNormalized);

            m_BattleModel.AddBattleEntity(battleEntitiy);

            await new WaitForSeconds(moveTime);

            fireballMoveModule.Stop();

            m_DamageController.DealDamage(senderID, targetID, m_FireballModule.Damage);

            BlowFireball(effectModule);

            m_BattleModel.RemoveBattleEntity(battleEntitiy.ID);
        }

        private float GetTimeToNextTick()
        {
            if (m_RhytmController.InputTickResult == EnumsCollection.InputTickResult.PreTick)
                return (float)m_RhytmController.TickDurationSeconds + -(float)m_RhytmController.DeltaInput;

            return (float)m_RhytmController.TimeToNextTick;
        }

        private void BlowFireball(EffectModule effectModule)
        {
            DataContainer data = new DataContainer();
            data.AddString("action", "blow");

            effectModule.EffectAction(data);
        }
    }
}