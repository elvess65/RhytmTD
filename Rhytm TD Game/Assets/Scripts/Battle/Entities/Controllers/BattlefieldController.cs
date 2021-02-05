using CoreFramework;
using CoreFramework.Abstract;
using RhytmTD.Battle.Entities.Models;
using System.Collections.Generic;
using System.Text;

namespace RhytmTD.Battle.Entities.Controllers
{
    /// <summary>
    /// Остлеживание ситуации на поле боя
    /// </summary>
    public class BattlefieldController : BaseController, iUpdatable
    {
        private BattleModel m_BattleModel;
        private DamageController m_DamageController;
        private List<int> m_EnemyAttackersContainer;

        private const float m_ENEMY_ATTACK_Z_DISTANCE = 0;

        public BattlefieldController(Dispatcher dispatcher) : base(dispatcher)
        {
        }

        public override void InitializeComplete()
        {
            m_BattleModel = Dispatcher.GetModel<BattleModel>();
            m_DamageController = Dispatcher.GetController<DamageController>();
            m_EnemyAttackersContainer = new List<int>();
        }

        public BattleEntity FindClosestTo(BattleEntity targetEntity)
        {
            if (!targetEntity.HasModule<TransformModule>())
                return null;

            BattleEntity result = null;
            float closestSQRDist = float.MaxValue;
            TransformModule targetTransformModule = targetEntity.GetModule<TransformModule>();

            foreach (BattleEntity entity in m_BattleModel.BattleEntities)
            {
                if (targetEntity.ID == entity.ID || !entity.HasModule<TransformModule>())
                    continue;

                TransformModule entityTransformModule = entity.GetModule<TransformModule>();
                float sqrDist = (targetTransformModule.Transform.position - entityTransformModule.Transform.position).sqrMagnitude;
                if (sqrDist < closestSQRDist)
                {
                    result = entity;
                    closestSQRDist = sqrDist;
                }
            }

            return result;
        }

        public void PerformUpdate(float deltaTime)
        {
            if (m_BattleModel.PlayerEntity == null || !m_BattleModel.PlayerEntity.HasModule<TransformModule>())
                return;

            TransformModule playerTransformModule = m_BattleModel.PlayerEntity.GetModule<TransformModule>();
            foreach (BattleEntity entity in m_BattleModel.BattleEntities)
            {
                if (m_BattleModel.PlayerEntity.ID == entity.ID || !entity.HasModule<TransformModule>())
                    continue;

                TransformModule entityTransformModule = entity.GetModule<TransformModule>();
                float zDist = entityTransformModule.Transform.position.z - playerTransformModule.Transform.position.z;
                if (zDist <= m_ENEMY_ATTACK_Z_DISTANCE)
                {
                    m_EnemyAttackersContainer.Add(entity.ID);
                }
            }

            foreach (int id in m_EnemyAttackersContainer)
            {
                m_DamageController.DealDamage(id, m_BattleModel.PlayerEntity.ID);
                m_BattleModel.RemoveBattleEntity(id);

                //TODO: Animations and post attack processing
            }
            m_EnemyAttackersContainer.Clear();
        }
    }
}
