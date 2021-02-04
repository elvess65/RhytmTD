using CoreFramework;
using RhytmTD.Battle.Entities.Models;

namespace RhytmTD.Battle.Entities.Controllers
{
    /// <summary>
    /// Остлеживание ситуации на поле боя
    /// </summary>
    public class BattlefieldController : BaseController
    {
        private BattleModel m_BattleModel;

        public BattlefieldController(Dispatcher dispatcher) : base(dispatcher)
        {
        }

        public override void InitializeComplete()
        {
            m_BattleModel = Dispatcher.GetModel<BattleModel>();
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
    }
}
