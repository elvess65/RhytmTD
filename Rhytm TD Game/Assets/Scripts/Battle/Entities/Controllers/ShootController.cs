using CoreFramework;
using RhytmTD.Assets.Battle;
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
        private Dictionary<int, BattleEntity> m_Bullets = new Dictionary<int, BattleEntity>();
        private List<int> m_BulletsToRemove = new List<int>();

        public ShootController(Dispatcher dispatcher) : base(dispatcher)
        {
        }

        public override void InitializeComplete()
        {
            m_BattleModel = Dispatcher.GetModel<BattleModel>();
            m_DamageController = Dispatcher.GetController<DamageController>();

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
            GameObject bulletObj = BattleAssetsManager.Instance.GetAssets().InstantiateGameObject(BattleAssetsManager.Instance.GetAssets().PlayerPrefab);

            BattleEntity bullet = new BattleEntity(IDGenerator.GenerateID());
            bullet.AddModule(new TransformModule(bulletObj.transform.position, bulletObj.transform.rotation));
            bullet.AddModule(new MoveModule(distanceToTarget / 60f)); /// BPMModel.BPM?
            bullet.AddModule(new TargetModule());
            bullet.AddModule(new OwnerModule { Owner = sender });

            return bullet;
        }

        private void Update(float t)
        {
            foreach (BattleEntity bullet in m_Bullets.Values)
            {
                TargetModule targetModule = bullet.GetModule<TargetModule>();
                MoveModule moveModule = bullet.GetModule<MoveModule>();
                OwnerModule ownerModule = bullet.GetModule<OwnerModule>();
                TransformModule ownerTransform = ownerModule.Owner.GetModule<TransformModule>();

                // Correct direction for bullets
                Vector3 dir = targetModule.TargetTransform.Position - ownerTransform.Position;
                moveModule.StartMove(dir.normalized);

                if (dir.sqrMagnitude <= DISTANCE_TO_HIT * DISTANCE_TO_HIT)
                {
                    m_DamageController.DealDamage(ownerModule.Owner.ID, targetModule.TargetID);

                    m_BulletsToRemove.Add(bullet.ID);
                    m_BattleModel.RemoveBattleEntity(bullet.ID);
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
