using CoreFramework;
using RhytmTD.Setup;

namespace RhytmTD.Network
{
    /// <summary>
    /// Контроллер подключения и получения данных
    /// </summary>
    public class ConnectionController
    {
        public System.Action OnConnectionSuccess;
        public System.Action<int> OnConnectionError;

        private ConnectionProxy m_ConnectionProxy;


        public ConnectionController()
        {
            m_ConnectionProxy = new ConnectionProxy();
            m_ConnectionProxy.OnConnectionSuccess += ConnectionResultSuccess;
            m_ConnectionProxy.OnConnectionError += ConnectionResultError;
        }

        public void Connect()
        {
            m_ConnectionProxy.Connect();
        }


        private void ConnectionResultSuccess(ConnectionSeccessResult connectionResult)
        {
            IGameSetup gameSetup = new GameSetup(new DataGameSetup(connectionResult.SerializedAccountData, 
                                                                   connectionResult.SerializedEnviromentData, 
                                                                   connectionResult.SerializedLevelingData, 
                                                                   connectionResult.SerializedWorldData,
                                                                   connectionResult.SerializedAccountBaseParamsData),
                                                 new BattleGameSetup());
            gameSetup.Setup();

            OnConnectionSuccess?.Invoke();
        }

        private void ConnectionResultError(int errorCode)
        {
            OnConnectionError?.Invoke(errorCode);
        }
    }
}
