using CoreFramework;
using RhytmTD.Battle.Entities.Models;

namespace RhytmTD.Battle.Entities.Controllers
{
    /// <summary>
    /// Контроллер прогресса боя
    /// </summary>
    public class BattleProgressController : BaseController
    {
        private BattleModel m_BattleModel;

        public BattleProgressController(Dispatcher dispatcher) : base(dispatcher)
        {
        }

        public override void InitializeComplete()
        {
            base.InitializeComplete();

            m_BattleModel = Dispatcher.GetModel<BattleModel>();
            m_BattleModel.OnPlayerEntityInitialized += PlayerEntityInitializedHandler;
        }

        private void PlayerEntityInitializedHandler(BattleEntity playerEntity)
        {
            playerEntity.GetModule<HealthModule>().OnDestroyed += PlayerEntity_OnDestroyed;
        }

        private void PlayerEntity_OnDestroyed(int entityID)
        {
            UnityEngine.Debug.LogError("Battle finished");
            //m_BattleModel.RemoveBattleEntity(entityID);
            //m_BattleModel.PlayerEntity = null;

            m_BattleModel.OnBattleFinished?.Invoke(true);
        }
    }
}
