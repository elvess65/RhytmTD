using RhytmTD.Battle.Entities.Controllers;
using RhytmTD.Battle.Entities.Models;

namespace RhytmTD.Core
{
    public class BattleGameSetup : IGameSetup
    {
        public void SetupDispatcher()
        {
            Dispatcher dispatcher = Dispatcher.Instance;

            // Controllers
            dispatcher.CreateController<MoveController>();

            // Models
            dispatcher.CreateModel<BattleModel>();
        }
    }
}
