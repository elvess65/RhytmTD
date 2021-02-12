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
        private DamageController m_DamageController;
        private RhytmController m_RhytmController;
        private SpawnController m_SpawnController;
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
            m_SpawnController = Dispatcher.GetController<SpawnController>();

            UpdateModel updateModel = Dispatcher.GetModel<UpdateModel>();
            updateModel.OnUpdate += Update;
        }

        public void Shoot(BattleEntity sender, BattleEntity target)
        {
            TransformModule senderTransform = sender.GetModule<TransformModule>();
            TransformModule targetTransform = target.GetModule<TransformModule>();

            Vector3 targetDir = senderTransform.Position - targetTransform.Position;
            BattleEntity bullet = CreateBullet(sender, targetDir.magnitude);

            bullet.GetModule<TargetModule>().SetTarget(target.ID, targetTransform);
            bullet.GetModule<MoveModule>().StartMove(targetDir.normalized);

            m_Bullets.Add(bullet.ID, bullet);

            m_BattleModel.AddBattleEntity(bullet);
        }

        private BattleEntity CreateBullet(BattleEntity sender, float distanceToTarget)
        {
            TransformModule senderTransform = sender.GetModule<TransformModule>();

            float speed = distanceToTarget / (float)m_RhytmController.TimeToNextTick;

            BattleEntity bullet = m_SpawnController.CreateBullet(1, senderTransform.Position, Quaternion.identity, speed, sender);

            return bullet;
        }

        private void Update(float t)
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
                    if (m_BattleModel.HasEntity(targetModule.TargetID))
                    {
                        m_DamageController.DealDamage(ownerModule.Owner.ID, targetModule.TargetID);
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
    }
}
