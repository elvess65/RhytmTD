using CoreFramework;
using RhytmTD.Data.Controllers;

namespace RhytmTD.Data
{
    /// <summary>
    /// Setup подключения
    /// </summary>
    public class ConnectionGameSetup : IGameSetup
    {
        public void Setup()
        {
            Dispatcher dispatcher = Dispatcher.Instance;

            dispatcher.CreateController<ConnectionController>();
        }
    }
}
