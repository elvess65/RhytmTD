using CoreFramework;
using RhytmTD.Battle.Core;
<<<<<<< HEAD:Rhytm TD Game/Assets/Scripts/Network/ConnectionController.cs
using RhytmTD.Data.DataBase;

namespace RhytmTD.Network
=======

namespace RhytmTD.Data.Connection
>>>>>>> 0bf768c11b6da08d921094585b36103d99dac912:Rhytm TD Game/Assets/Scripts/Data/Connection/ConnectionController.cs
{
    /// <summary>
    /// Контроллер подключения и получения данных
    /// </summary>
    public class ConnectionController
    {
        public System.Action OnConnectionSuccess;
        public System.Action<int> OnConnectionError;

        private ConnectionProxy m_ConnectionProxy;

<<<<<<< HEAD:Rhytm TD Game/Assets/Scripts/Network/ConnectionController.cs
=======

        public ConnectionController(Dispatcher dispatcher) : base(dispatcher)
        {
            m_ConnectionProxy = new ConnectionProxy();
            m_ConnectionProxy.OnConnectionSuccess += ConnectionResultSuccess;
            m_ConnectionProxy.OnConnectionError += ConnectionResultError;
        }

>>>>>>> 0bf768c11b6da08d921094585b36103d99dac912:Rhytm TD Game/Assets/Scripts/Data/Connection/ConnectionController.cs
        public void Connect()
        {
            m_ConnectionProxy.Connect();
        }

        private void ConnectionResultSuccess(string serializedAccountData, string serializedEnviromentData, string serializedLevelingData, string serializedWorldData)
        {
            IGameSetup gameSetup = new DataGameSetup(serializedAccountData, serializedEnviromentData, serializedLevelingData, serializedWorldData);
            gameSetup.Setup();

<<<<<<< HEAD:Rhytm TD Game/Assets/Scripts/Network/ConnectionController.cs
            IGameSetup gameSetup = new GameSetup(new DataGameSetup(serializedAccountData, serializedEnviromentData, serializedLevelingData, serializedWorldData),
                                                 new BattleGameSetup());
            gameSetup.Setup();
=======
            IGameSetup battleSetup = new BattleGameSetup();
            battleSetup.Setup();
>>>>>>> 0bf768c11b6da08d921094585b36103d99dac912:Rhytm TD Game/Assets/Scripts/Data/Connection/ConnectionController.cs

            OnConnectionSuccess?.Invoke();
        }

        private void ConnectionResultError(int errorCode)
        {
            OnConnectionError?.Invoke(errorCode);
        }
    }
}
