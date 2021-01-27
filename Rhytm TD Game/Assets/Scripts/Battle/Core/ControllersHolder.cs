using CoreFramework.Input;
using CoreFramework.Rhytm;
using RhytmTD.Battle.Spawn;

namespace RhytmTD.Battle.Core
{
    /// <summary>
    /// Holder for all controllers
    /// </summary>
    public class ControllersHolder
    {
        public RhytmController RhytmController { get; private set; }
        public RhytmInputProxy RhytmInputProxy { get; private set; }
        public InputController InputController { get; private set; }
        public SpawnController SpawnController { get; private set; }

        public ControllersHolder()
        {
            RhytmController = new RhytmController();
            RhytmInputProxy = new RhytmInputProxy();
            InputController = new InputController();
            SpawnController = new SpawnController();
        }
    }
}
