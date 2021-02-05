using CoreFramework;
using CoreFramework.Input;
using CoreFramework.Rhytm;
using RhytmTD.Battle.Entities.Controllers;
using RhytmTD.Battle.Entities.Models;
using RhytmTD.Battle.Spawn;

namespace RhytmTD.Battle.Core
{
    /// <summary>
    /// Setup для боя
    /// </summary>
    public class BattleGameSetup : IGameSetup
    {
        public void Setup()
        {
            Dispatcher dispatcher = Dispatcher.Instance;

            // Controllers
            dispatcher.CreateController<MoveController>();
            dispatcher.CreateController<FocusController>();
            dispatcher.CreateController<RhytmController>();
            dispatcher.CreateController<RhytmInputProxy>();
            dispatcher.CreateController<InputController>();
            dispatcher.CreateController<SpawnController>();
            dispatcher.CreateController<DamageController>();
            dispatcher.CreateController<BattlefieldController>();

            // Models
            dispatcher.CreateModel<BattleModel>();

            dispatcher.InitializeComplete();
        }
    }
}
