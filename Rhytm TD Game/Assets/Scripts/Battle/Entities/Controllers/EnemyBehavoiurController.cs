using CoreFramework;
using CoreFramework.Rhytm;
using RhytmTD.Battle.Entities.Models;
using UnityEngine;

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
        private RhytmController m_RhytmController;

        private const float m_ENEMY_FOCUS_Z_DISTANCE = 10;
        private const float m_ENEMY_MOVE_Z_DISTANCE = 2f;
        private const float m_ENEMY_ATTACK_DISTANCE = 2f;
        private const float m_MOVE_TO_ENEMY_MIN_TIME = 0.1f;

        public EnemyBehavoiurController(Dispatcher dispatcher) : base(dispatcher)
        {
        }

        public override void InitializeComplete()
        {
            m_SpawnModel = Dispatcher.GetModel<SpawnModel>();
            m_BattleModel = Dispatcher.GetModel<BattleModel>();
            m_BattleModel.OnPlayerEntityInitialized += PlayerInitializedHandler;

            m_DamageController = Dispatcher.GetController<DamageController>();
            m_RhytmController = Dispatcher.GetController<RhytmController>();
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
                MoveModule moveModule = entity.GetModule<MoveModule>();

                float zDist = entityTransformModule.Position.z - playerTransform.Position.z;

                if (zDist <= m_ENEMY_FOCUS_Z_DISTANCE && zDist > m_ENEMY_MOVE_Z_DISTANCE)
                {
                    if (!focusModule.IsFocusing)
                    {
                        focusModule.StartFocusOnTarget(m_BattleModel.PlayerEntity.ID, playerTransform);
                        animationModule.PlayAnimation(EnumsCollection.AnimationTypes.IdleBattle);
                    }
                }
                else if (zDist <= m_ENEMY_MOVE_Z_DISTANCE && !moveModule.IsMoving)
                {
                    Vector3 enemyDirection = playerTransform.Position - entityTransformModule.Position;
                    StartMoveToEnemy(enemyDirection, moveModule, animationModule);
                }
                else if (moveModule.IsMoving)
                {
                    Vector3 enemyDirection = playerTransform.Position - entityTransformModule.Position;

                    if (enemyDirection.sqrMagnitude <= m_ENEMY_ATTACK_DISTANCE * m_ENEMY_ATTACK_DISTANCE)
                    {
                        AttackEnemy(entity, moveModule, focusModule, animationModule);
                    }
                    else
                    {
                        moveModule.StartMove(enemyDirection.normalized);
                    }
                }
            }
        }

        private void StartMoveToEnemy(Vector3 enemyDirection, MoveModule moveModule, AnimationModule animationModule)
        {
            float moveTime = m_RhytmController.GetTimeToNextTick() - animationModule.AttackAnimationTime;

            if (moveTime <= m_MOVE_TO_ENEMY_MIN_TIME)
            {
                moveTime += (float)m_RhytmController.TickDurationSeconds;
            }

            float distanceToTarget = enemyDirection.magnitude;

            moveModule.SetSpeed(distanceToTarget / moveTime);
            moveModule.StartMove(enemyDirection / distanceToTarget);
        }

        private void AttackEnemy(BattleEntity sender, MoveModule moveModule, FocusModule focusModule, AnimationModule animationModule)
        {
            moveModule.Stop();
            focusModule.StopFocus();

            sender.RemoveModule<EnemyBehaviourTag>();

            animationModule.PlayAnimation(EnumsCollection.AnimationTypes.Attack);
            m_DamageController.DealDamage(sender.ID, m_BattleModel.PlayerEntity.ID);

            m_SpawnModel.OnEnemyRemoved(sender);
        }
    }
}
