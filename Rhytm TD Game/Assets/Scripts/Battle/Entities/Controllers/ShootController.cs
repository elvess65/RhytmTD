﻿using CoreFramework;
using CoreFramework.Rhytm;
using RhytmTD.Battle.Entities.Models;
using RhytmTD.Data;
using RhytmTD.Data.Models;
using System.Collections.Generic;
using UnityEngine;

namespace RhytmTD.Battle.Entities.Controllers
{
    public class ShootController : BaseController
    {
        private const float DISTANCE_TO_HIT_TARGET = 0.1f;
        private const float NO_TARGET_BULLET_FLY_DISTANCE = 10;
        private const int NO_TARGET_BULLET_EXISTS_TICKS = 2;

        private BattleModel m_BattleModel;
        private RhytmController m_RhytmController;
        private DamageController m_DamageController;
        private EffectsController m_EffectsController;
        
        private List<int> m_BulletsToRemoveBuffer = new List<int>();
        private Dictionary<int, int> m_BulletsWithoutTarget = new Dictionary<int, int>(); 
        private Dictionary<int, BattleEntity> m_Bullets = new Dictionary<int, BattleEntity>();

        public ShootController(Dispatcher dispatcher) : base(dispatcher)
        {
        }

        public override void InitializeComplete()
        {
            m_BattleModel = Dispatcher.GetModel<BattleModel>();
            m_DamageController = Dispatcher.GetController<DamageController>();
            m_EffectsController = Dispatcher.GetController<EffectsController>();
            m_RhytmController = Dispatcher.GetController<RhytmController>();
            m_RhytmController.OnTick += HandleTick;

            UpdateModel updateModel = Dispatcher.GetModel<UpdateModel>();
            updateModel.OnUpdate += HandleUpdate;
        }

        public void Shoot(BattleEntity sender, BattleEntity target)
        {
            Vector3 targetDir = GetTargetDir(sender, target);
            float distanceToTarget = targetDir.magnitude;
            float speed = distanceToTarget / m_RhytmController.GetTimeToNextTick();
            Vector3 targetDirNorm = targetDir / distanceToTarget;

            BattleEntity bullet = CreateBullet(sender, targetDirNorm, speed);

            bullet.GetModule<TargetModule>().SetTarget(target);
            bullet.GetModule<MoveModule>().StartMove(targetDirNorm);

            m_Bullets.Add(bullet.ID, bullet);
            m_BattleModel.AddBattleEntity(bullet);
        }

        public void Shoot(BattleEntity sender, Vector3 targetDir)
        {
            float speed = NO_TARGET_BULLET_FLY_DISTANCE / m_RhytmController.GetTimeToNextTick();

            BattleEntity bullet = CreateBullet(sender, targetDir, speed);

            bullet.GetModule<MoveModule>().StartMove(targetDir);

            m_BulletsWithoutTarget.Add(bullet.ID, m_RhytmController.CurrentTick + NO_TARGET_BULLET_EXISTS_TICKS);
            m_BattleModel.AddBattleEntity(bullet);
        }

     
        private void HandleUpdate(float deltaTime)
        {
            HandleTargetBasedHitTrack(deltaTime);
            HandleBulletRemove();
        }

        private void HandleTick(int ticksSinceStart)
        {
            if (m_BulletsWithoutTarget.Count == 0)
                return;

            foreach (int bulletID in m_BulletsWithoutTarget.Keys)
            {
                if (ticksSinceStart == m_BulletsWithoutTarget[bulletID])
                {
                    BufferBulletRemove(m_BattleModel.GetEntity(bulletID));
                }
            }
        }

        private void HandleTargetBasedHitTrack(float deltaTime)
        {
            foreach (BattleEntity bullet in m_Bullets.Values)
            {
                TargetModule targetModule = bullet.GetModule<TargetModule>();

                if (targetModule.Target == null)
                    continue;

                MoveModule moveModule = bullet.GetModule<MoveModule>();
                OwnerModule ownerModule = bullet.GetModule<OwnerModule>();
                TransformModule transformModule = bullet.GetModule<TransformModule>();

                SlotModule targetSlotModule = targetModule.Target.GetModule<SlotModule>();

                // Correct direction for bullets
                Vector3 dir = targetSlotModule.HitSlot.position - transformModule.Position;
                moveModule.StartMove(dir.normalized);

                if (dir.sqrMagnitude <= DISTANCE_TO_HIT_TARGET * DISTANCE_TO_HIT_TARGET)
                {
                    if (m_BattleModel.HasEntity(targetModule.Target.ID))
                    {
                        DamageModule damageModule = bullet.GetModule<DamageModule>();
                        m_DamageController.DealDamage(ownerModule.Owner.ID, targetModule.Target.ID, damageModule.MaxDamage);
                    }

                    BufferBulletRemove(bullet);
                }
            }
        }

        private void HandleBulletRemove()
        {
            if (m_BulletsToRemoveBuffer.Count > 0)
            {
                foreach (int bulletID in m_BulletsToRemoveBuffer)
                {
                    m_Bullets.Remove(bulletID);

                    if (m_BulletsWithoutTarget.ContainsKey(bulletID))
                        m_BulletsWithoutTarget.Remove(bulletID);
                }

                m_BulletsToRemoveBuffer.Clear();
            }
        }

        private void BufferBulletRemove(BattleEntity bullet)
        {
            m_BulletsToRemoveBuffer.Add(bullet.ID);

            DestroyModule destroyModule = bullet.GetModule<DestroyModule>();
            destroyModule.SetDestroyed();

            //WARNING - EXTRA ALLOCATION. CAUSE GARBAGE
            DataContainer data = new DataContainer();
            data.AddObject(ConstsCollection.DataConsts.ACTION, ConstsCollection.DataConsts.EXPLOSION);

            EffectModule effectModule = bullet.GetModule<EffectModule>();
            effectModule.EffectAction(data);
        }

        private void BulletDestroyedHandler(BattleEntity battleEntity)
        {
            m_BattleModel.RemoveBattleEntity(battleEntity.ID);
        }

        private Vector3 GetTargetDir(BattleEntity sender, BattleEntity target)
        {
            SlotModule senderSlotModule = sender.GetModule<SlotModule>();
            SlotModule targetSlotModule = target.GetModule<SlotModule>();

            return targetSlotModule.HitSlot.position - senderSlotModule.ProjectileSlot.position;
        }

        private BattleEntity CreateBullet(BattleEntity sender, Vector3 vecToTarget, float speed)
        {
            SlotModule senderSlot = sender.GetModule<SlotModule>();
            DamageModule senderDamageModule = sender.GetModule<DamageModule>();

            Quaternion rotation = Quaternion.LookRotation(vecToTarget);

            BattleEntity bulletEnity = m_EffectsController.CreateBulletEffect(ConstsCollection.EffectConsts.ProjectileArrow,
                                                                              senderSlot.ProjectileSlot.position,
                                                                              rotation, speed, sender);

            DamageModule damageModule = bulletEnity.GetModule<DamageModule>();
            damageModule.MinDamage = damageModule.MaxDamage = senderDamageModule.RandomDamage();

            DestroyModule destroyModule = bulletEnity.GetModule<DestroyModule>();
            destroyModule.OnDestroyed += BulletDestroyedHandler;

            if (sender.HasModule<DamagePredictionModule>())
            {
                DamagePredictionModule damagePredictionModule = sender.GetModule<DamagePredictionModule>();
                damagePredictionModule.PotentialDamage += damageModule.MaxDamage;
            }

            return bulletEnity;
        }
    }
}
