using CoreFramework;
using RhytmTD.Battle.Entities.Models;
using RhytmTD.Data.Models;

namespace RhytmTD.Battle.Entities.Controllers
{
    public class EnemyBehavoiurController : BaseController
    {
        private BattleModel m_BattleModel;
        private DamageController m_DamageController;
        private DestroyModule m_PlayerDestroyModule;
        private UpdateModel m_UpdateModel;

        private const float m_ENEMY_FOCUS_Z_DISTANCE = 10;
        private const float m_ENEMY_ATTACK_Z_DISTANCE = 0;

        public EnemyBehavoiurController(Dispatcher dispatcher) : base(dispatcher)
        {
        }

        public override void InitializeComplete()
        {
            m_BattleModel = Dispatcher.GetModel<BattleModel>();
            m_DamageController = Dispatcher.GetController<DamageController>();

            m_BattleModel.OnPlayerEntityInitialized += PlayerInitializedHandler;
        }

        private void PlayerInitializedHandler(BattleEntity battleEntity)
        {
            m_UpdateModel = Dispatcher.GetModel<UpdateModel>();
            m_UpdateModel.OnUpdate += Update;

            m_BattleModel.OnBattleFinished += OnBattleFinishedHandler;
        }

        private void OnBattleFinishedHandler(bool isSuccess)
        {
            m_UpdateModel.OnUpdate -= Update;
        }

        private void Update(float deltaTime)
        {
            TransformModule playerTransform = m_BattleModel.PlayerEntity.GetModule<TransformModule>();

            foreach (BattleEntity entity in m_BattleModel.BattleEntities)
            {
                if (!entity.HasModule<EnemyBehaviourTag>())
                    continue;

                FocusModule focusModule = entity.GetModule<FocusModule>();
                TransformModule entityTransformModule = entity.GetModule<TransformModule>();
                
                float zDist = entityTransformModule.Position.z - playerTransform.Position.z;

                if (zDist <= m_ENEMY_FOCUS_Z_DISTANCE && zDist > m_ENEMY_ATTACK_Z_DISTANCE)
                {
                    focusModule.StartFocusOnTarget(m_BattleModel.PlayerEntity.ID, playerTransform);
                }
                else if (zDist <= m_ENEMY_ATTACK_Z_DISTANCE)
                {
                    focusModule.StopFocus();
                    entity.RemoveModule<EnemyBehaviourTag>();

                    m_DamageController.DealDamage(entity.ID, m_BattleModel.PlayerEntity.ID);
                }
            }
        }
    }
}
