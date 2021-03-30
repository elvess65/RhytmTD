using CoreFramework;
using RhytmTD.Battle.Entities.Models;

namespace RhytmTD.Battle.Entities.Controllers
{
    public class SyncPositionController : BaseController
    {
        private BattleModel m_BattleModel;
        private UpdateModel m_UpdateModel;

        public SyncPositionController(Dispatcher dispatcher) : base(dispatcher)
        {
        }

        public override void InitializeComplete()
        {
            m_BattleModel = Dispatcher.GetModel<BattleModel>();

            m_UpdateModel = Dispatcher.GetModel<UpdateModel>();
            m_UpdateModel.OnUpdate += Update;
        }

        private void Update(float deltaTime)
        {
            foreach (BattleEntity battleEntity in m_BattleModel.BattleEntities)
            {
                if (battleEntity.HasModule<SyncPositionModule>())
                {
                    SyncPositionModule syncPositionModule = battleEntity.GetModule<SyncPositionModule>();

                    TransformModule transformModule = battleEntity.GetModule<TransformModule>();
                    TransformModule targetTransformModule = syncPositionModule.Target.GetModule<TransformModule>();

                    transformModule.Position = targetTransformModule.Position;
                }
            }
        }

        public override void Dispose()
        {
            base.Dispose();

            m_UpdateModel.OnUpdate -= Update;
        }
    }
}
