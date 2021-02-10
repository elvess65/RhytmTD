using CoreFramework;

namespace RhytmTD.Data.Connection
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
