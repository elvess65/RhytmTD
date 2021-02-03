using CoreFramework;
using RhytmTD.Battle.Entities.Models;

namespace RhytmTD.Battle.Entities.Controllers
{
    public class MoveController : BaseController
    {
        BattleModel m_BattleModel;

        public MoveController(Dispatcher dispatcher) : base(dispatcher)
        {
        }

        public override void InitializeComplete()
        {
            m_BattleModel = Dispatcher.GetModel<BattleModel>();
        }

        public void MoveAll(float deltaTime)
        {
            foreach (BattleEntity entity in m_BattleModel.BattleEntities)
            {
                if (entity.HasModule<MoveModule>())
                {
                    MoveModule moveModule = entity.GetModule<MoveModule>();

                    if (moveModule.IsMoving)
                    {
                        moveModule.Transform.position += moveModule.MoveDirection * moveModule.CurrentSpeed * deltaTime;
                    }
                }
            }
        }
    }
}
