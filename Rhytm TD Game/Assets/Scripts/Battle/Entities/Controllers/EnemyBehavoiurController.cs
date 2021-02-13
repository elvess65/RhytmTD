﻿using CoreFramework;
using RhytmTD.Battle.Entities.Models;
using RhytmTD.Data.Models;

namespace RhytmTD.Battle.Entities.Controllers
{
    public class EnemyBehavoiurController : BaseController
    {
        private BattleModel m_BattleModel;
        private DamageController m_DamageController;

        private const float m_ENEMY_FOCUS_Z_DISTANCE = 10;
        private const float m_ENEMY_ATTACK_Z_DISTANCE = 0;

        public EnemyBehavoiurController(Dispatcher dispatcher) : base(dispatcher)
        {
        }

        public override void InitializeComplete()
        {
            m_BattleModel = Dispatcher.GetModel<BattleModel>();

            UpdateModel updateModel = Dispatcher.GetModel<UpdateModel>();
            updateModel.OnUpdate += Update;

            m_DamageController = Dispatcher.GetController<DamageController>(); 
        }

        private void Update(float deltaTime)
        {
            if (m_BattleModel.PlayerEntity == null || !m_BattleModel.PlayerEntity.GetModule<HealthModule>().IsAlive)
                return;

            TransformModule playerTransform = m_BattleModel.PlayerEntity.GetModule<TransformModule>();

            foreach (BattleEntity entity in m_BattleModel.BattleEntities)
            {
                if (!m_BattleModel.PlayerEntity.GetModule<HealthModule>().IsAlive || !entity.HasModule<EnemyBehaviourTag>())
                    continue;

                TransformModule entityTransformModule = entity.GetModule<TransformModule>();
                float zDist = entityTransformModule.Position.z - playerTransform.Position.z;

                if (zDist <= m_ENEMY_FOCUS_Z_DISTANCE && zDist > m_ENEMY_ATTACK_Z_DISTANCE)
                {
                    FocusModule focusModule = entity.GetModule<FocusModule>();
                    focusModule.StartFocusOnTarget(m_BattleModel.PlayerEntity.ID, playerTransform);
                }
                else if (zDist <= m_ENEMY_ATTACK_Z_DISTANCE)
                {
                    m_DamageController.DealDamage(entity.ID, m_BattleModel.PlayerEntity.ID);
                }
            }
        }
    }
}
