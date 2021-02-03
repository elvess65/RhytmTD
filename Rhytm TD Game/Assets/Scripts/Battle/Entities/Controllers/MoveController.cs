using CoreFramework;
using RhytmTD.Battle.Entities.Models;

namespace RhytmTD.Battle.Entities.Controllers
{
    public class MoveController : BaseController
    {
        public MoveController(Dispatcher dispatcher) : base(dispatcher)
        {
        }

        public void MoveAll(float deltaTime)
        {
            BattleModel battleModel = Dispatcher.GetModel<BattleModel>();

            foreach (BattleEntity entity in battleModel.BattleEntities)
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
