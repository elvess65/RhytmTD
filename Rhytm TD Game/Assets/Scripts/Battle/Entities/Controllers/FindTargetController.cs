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
            float nearestDistance = 0;
            BattleEntity retValue = null;

            foreach (BattleEntity entity in m_BattleModel.BattleEntities)
            {
                if (entity.ID == sender.ID || !entity.HasModule<TransformModule>())
                    continue;

                TransformModule transformModule = entity.GetModule<TransformModule>();

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
