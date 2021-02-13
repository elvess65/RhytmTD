using CoreFramework;
using RhytmTD.Battle.Entities.Models;

namespace RhytmTD.Battle.Entities.Controllers
{
    public class FindTargetController : BaseController
    {
        private BattleModel m_BattleModel;

        public FindTargetController(Dispatcher dispatcher) : base(dispatcher)
        {
        }

        public override void InitializeComplete()
        {
            m_BattleModel = Dispatcher.GetModel<BattleModel>();
        }

        public BattleEntity GetNearestTarget(BattleEntity sender)
        {
            TransformModule senderTransform = sender.GetModule<TransformModule>();
            float nearestDistance = float.MaxValue;
            BattleEntity retValue = null;

            foreach (BattleEntity entity in m_BattleModel.BattleEntities)
            {
                if (entity.ID == sender.ID || !entity.HasModule<TransformModule>() || entity.HasModule<PredictedDestroyedTag>())
                    continue;

                DestroyModule destroyModule = entity.GetModule<DestroyModule>();

                if (destroyModule.IsDestroyed)
                    continue;

                TransformModule transformModule = entity.GetModule<TransformModule>();

                if (senderTransform.Position.z >= transformModule.Position.z)
                    continue;

                float distanceSqr = (senderTransform.Position - transformModule.Position).sqrMagnitude;
                if (distanceSqr <= nearestDistance)
                {
                    nearestDistance = distanceSqr;
                    retValue = entity;
                }    
            }

            return retValue;
        }
    }
}
