using CoreFramework;
using RhytmTD.Battle.Entities.Models;
using RhytmTD.Data.Models;

namespace RhytmTD.Battle.Entities.Controllers
{
    /// <summary>
    /// Enemy behaviour (focusing and attack)
    /// </summary>
    public class EnemyBehavoiurController : BaseController
    {
        private UpdateModel m_UpdateModel;
        private BattleModel m_BattleModel;
        private SpawnModel m_SpawnModel;
        
        private DamageController m_DamageController;

        private const float m_ENEMY_FOCUS_Z_DISTANCE = 10;
        private const float m_ENEMY_ATTACK_Z_DISTANCE = 0;

        public EnemyBehavoiurController(Dispatcher dispatcher) : base(dispatcher)
        {
        }

        public override void InitializeComplete()
        {
            m_SpawnModel = Dispatcher.GetModel<SpawnModel>();
            m_BattleModel = Dispatcher.GetModel<BattleModel>();
            m_BattleModel.OnPlayerEntityInitialized += PlayerInitializedHandler;

            m_DamageController = Dispatcher.GetController<DamageController>();
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
                AnimationModule animationModule = entity.GetModule<AnimationModule>();
                TransformModule entityTransformModule = entity.GetModule<TransformModule>();
                
                float zDist = entityTransformModule.Position.z - playerTransform.Position.z;

                if (zDist <= m_ENEMY_FOCUS_Z_DISTANCE && zDist > m_ENEMY_ATTACK_Z_DISTANCE)
                {
                    if (!focusModule.IsFocusing)
                    {
                        focusModule.StartFocusOnTarget(m_BattleModel.PlayerEntity.ID, playerTransform);
                        animationModule.PlayAnimation(EnumsCollection.AnimationTypes.IdleBattle);
                    }
                }
                else if (zDist <= m_ENEMY_ATTACK_Z_DISTANCE)
                {
                    focusModule.StopFocus();
                    entity.RemoveModule<EnemyBehaviourTag>();

                    animationModule.PlayAnimation(EnumsCollection.AnimationTypes.Attack);
                    m_DamageController.DealDamage(entity.ID, m_BattleModel.PlayerEntity.ID);

                    m_SpawnModel.OnEnemyRemoved(entity);
                }
            }
        }
    }
}
