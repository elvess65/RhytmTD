using CoreFramework;
using CoreFramework.Rhytm;
using RhytmTD.Battle.Entities.Models;
using RhytmTD.Data.Models;
using System.Collections.Generic;
using UnityEngine;

namespace RhytmTD.Battle.Entities.Controllers
{
    public class ShootController : BaseController
    {
        private static float DISTANCE_TO_HIT = 0.1f; 

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
            BattleEntity bullet = CreateBullet(sender, targetDir);

            bullet.GetModule<TargetModule>().SetTarget(target);
            bullet.GetModule<MoveModule>().StartMove(targetDir.normalized);

            m_Bullets.Add(bullet.ID, bullet);

            m_BattleModel.AddBattleEntity(bullet);
        }

 
        private Vector3 GetTargetDir(BattleEntity sender, BattleEntity target)
        {
            TransformModule senderTransform = sender.GetModule<TransformModule>();
            TransformModule targetTransform = target.GetModule<TransformModule>();

            return senderTransform.Position - targetTransform.Position;
        }

        private BattleEntity CreateBullet(BattleEntity sender, Vector3 vecToTarget)
        {
            SlotModule senderSlot = sender.GetModule<SlotModule>();
            DamageModule senderDamageModule = sender.GetModule<DamageModule>();

            Quaternion rotation = Quaternion.LookRotation(vecToTarget);
            float speed = vecToTarget.magnitude / GetTimeToNextTick();
            
            BattleEntity bulletEnity = m_EffectsController.CreateBulletEffect(EnumsCollection.BattlEffectID.ProjectileArrow, senderSlot.ProjectileSlot.position, rotation, speed, sender);

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

        private float GetTimeToNextTick()
        {
            if (m_RhytmController.InputTickResult == EnumsCollection.InputTickResult.PreTick)
                return (float)m_RhytmController.TickDurationSeconds + -(float)m_RhytmController.DeltaInput;

            return (float)m_RhytmController.TimeToNextTick;
        }

        private void Update(float deltaTime)
        {
            foreach (BattleEntity bullet in m_Bullets.Values)
            {
                TargetModule targetModule = bullet.GetModule<TargetModule>();
                MoveModule moveModule = bullet.GetModule<MoveModule>();
                OwnerModule ownerModule = bullet.GetModule<OwnerModule>();
                TransformModule transformModule = bullet.GetModule<TransformModule>();

                // Correct direction for bullets
                Vector3 dir = targetModule.TargetTransform.Position - transformModule.Position;
                moveModule.StartMove(dir.normalized);

                if (dir.sqrMagnitude <= DISTANCE_TO_HIT * DISTANCE_TO_HIT)
                {
                    if (m_BattleModel.HasEntity(targetModule.Target.ID))
                    {
                        DamageModule damageModule = bullet.GetModule<DamageModule>();

                        m_DamageController.DealDamage(ownerModule.Owner.ID, targetModule.Target.ID, damageModule.MaxDamage);
                    }

                    m_BulletsToRemove.Add(bullet.ID);

                    DestroyModule destroyModule = bullet.GetModule<DestroyModule>();
                    destroyModule.SetDestroyed();
                }
            }

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
