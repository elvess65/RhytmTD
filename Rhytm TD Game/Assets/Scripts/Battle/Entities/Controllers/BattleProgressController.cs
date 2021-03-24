using CoreFramework;
using RhytmTD.Battle.Entities.Models;

namespace RhytmTD.Battle.Entities.Controllers
{
    /// <summary>
    /// Controlls battle progress and notifies about finishing
    /// </summary>
    public class BattleProgressController : BaseController
    {
        private BattleModel m_BattleModel;
        private SpawnModel m_SpawnModel;
        private BattleController m_BattleController;

        public BattleProgressController(Dispatcher dispatcher) : base(dispatcher)
        {
        }

        public override void InitializeComplete()
        {
            base.InitializeComplete();

            m_BattleModel = Dispatcher.GetModel<BattleModel>();
            m_BattleModel.OnPlayerEntityInitialized += PlayerEntityInitializedHandler;
            
            m_SpawnModel = Dispatcher.GetModel<SpawnModel>();
            m_SpawnModel.OnEnemyRemoved += EnemyEntity_OnRemoved;

            m_BattleController = Dispatcher.GetController<BattleController>();
        }

        private void PlayerEntityInitializedHandler(BattleEntity playerEntity)
        {
            playerEntity.GetModule<DestroyModule>().OnDestroyed += PlayerEntity_OnDestroyed;
        }

        private void PlayerEntity_OnDestroyed(BattleEntity entity)
        {
            m_BattleController.FinishBattle(false);
        }

        private void EnemyEntity_OnRemoved(BattleEntity entity)
        {
            CheckBattleState();
        }

        private void CheckBattleState()
        {
            if (GetExistingEnemiesAmount() == 0 && m_SpawnModel.IsBattleSpawnFinished)
            {
                m_BattleController.FinishBattle(true);
            }
        }

        private int GetExistingEnemiesAmount()
        {
            int enemiesAmount = 0;

            foreach (BattleEntity entity in m_BattleModel.BattleEntities)
            {
                if (entity.ID == m_BattleModel.ID || !entity.HasModule<EnemyBehaviourTag>() || entity.HasModule<PredictedDestroyedTag>())
                    continue;

                enemiesAmount++;
            }

            return enemiesAmount;
        }
    }
}
