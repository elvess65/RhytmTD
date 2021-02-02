

using RhytmTD.Battle.Entities.Controllers;

namespace RhytmTD.Core
{
    public class DefaultGameSetup : IGameSetup
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
