using CoreFramework;
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
        private static float DISTANCE_TO_HIT_TARGET = 0.1f;
        private static float DISTANCE_TO_HIT_DIRECTION = 2f;

        private BattleModel m_BattleModel;
        private RhytmController m_RhytmController;
        private DamageController m_DamageController;
        private EffectsController m_EffectsController;
        private Dictionary<int, BattleEntity> m_Bullets = new Dictionary<int, BattleEntity>();
        private List<int> m_BulletsToRemove = new List<int>();

        public ShootController(Dispatcher dispatcher) : base(dispatcher)
        {
        }

        public override void InitializeComplete()
        {
            m_BattleModel = Dispatcher.GetModel<BattleModel>();
            m_DamageController = Dispatcher.GetController<DamageController>();
            m_RhytmController = Dispatcher.GetController<RhytmController>();
            m_EffectsController = Dispatcher.GetController<EffectsController>();

            UpdateModel updateModel = Dispatcher.GetModel<UpdateModel>();
            updateModel.OnUpdate += Update;
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
            float existTimeTicks = 5;
            float speed = existTimeTicks * (float)m_RhytmController.TickDurationSeconds;

            BattleEntity bullet = CreateBullet(sender, targetDir, speed);

            bullet.GetModule<MoveModule>().StartMove(targetDir);

            m_Bullets.Add(bullet.ID, bullet);

            m_BattleModel.AddBattleEntity(bullet);
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

        private void Update(float deltaTime)
        {
#if TARGET_BASED_ATTACK
            DirectionBasedUpdate(deltaTime);   
#elif DIRECTION_BASED_ATTACK
            DirectionBasedUpdate(deltaTime);
#endif

            HandleBulletRemove();
        }

        private void TargetBasedUpdate(float deltaTime)
        {
            foreach (BattleEntity bullet in m_Bullets.Values)
            {
                TargetModule targetModule = bullet.GetModule<TargetModule>();
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

                    HandleBulletRemove(bullet);
                }
            }
        }

        private void DirectionBasedUpdate(float deltaTime)
        {
            foreach (BattleEntity bullet in m_Bullets.Values)
            {
                MoveModule moveModule = bullet.GetModule<MoveModule>();
                OwnerModule ownerModule = bullet.GetModule<OwnerModule>();
                TransformModule transformModule = bullet.GetModule<TransformModule>();
                Vector3 mapped2GroundBulletPos = transformModule.Position;
                mapped2GroundBulletPos.y = 0;

                foreach(BattleEntity entity in m_BattleModel.BattleEntities)
                {
                    if (!entity.HasModule<EnemyBehaviourTag>())
                        continue;

                    TransformModule enemyTransformModule = entity.GetModule<TransformModule>();
                    Vector3 mapped2GroundEnemyPos = enemyTransformModule.Position;
                    mapped2GroundEnemyPos.y = 0;

                    Vector3 dist2Enemy = mapped2GroundBulletPos - mapped2GroundEnemyPos;
                    if (dist2Enemy.sqrMagnitude <= DISTANCE_TO_HIT_DIRECTION * DISTANCE_TO_HIT_DIRECTION)
                    {
                        DamageModule damageModule = bullet.GetModule<DamageModule>();
                        m_DamageController.DealDamage(ownerModule.Owner.ID, entity.ID, damageModule.MaxDamage);

                        HandleBulletRemove(bullet);
                    }
                }
            }
        }

        private void HandleBulletRemove(BattleEntity bullet)
        {
            m_BulletsToRemove.Add(bullet.ID);

            DestroyModule destroyModule = bullet.GetModule<DestroyModule>();
            destroyModule.SetDestroyed();

            //WARNING - EXTRA ALLOCATION. CAUSE GARBAGE
            DataContainer data = new DataContainer();
            data.AddObject(ConstsCollection.DataConsts.ACTION, ConstsCollection.DataConsts.EXPLOSION);

            EffectModule effectModule = bullet.GetModule<EffectModule>();
            effectModule.EffectAction(data);
        }

        private void HandleBulletRemove()
        {
            if (m_BulletsToRemove.Count > 0)
            {
                foreach (int bulletID in m_BulletsToRemove)
                {
                    m_Bullets.Remove(bulletID);
                }

                m_BulletsToRemove.Clear();
            }
        }

        private void BulletDestroyedHandler(BattleEntity battleEntity)
        {
            m_BattleModel.RemoveBattleEntity(battleEntity.ID);
        }
    }
}
