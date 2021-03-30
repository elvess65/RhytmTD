using RhytmTD.Setup;

namespace CoreFramework.Network
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


        protected virtual IGameSetup CreateSetup()
        {
            return null;
        }

        private void ConnectionResultSuccess(ConnectionSeccessResult connectionResult)
        {
            IGameSetup gameSetup = new GameSetup(new DataGameSetup(connectionResult.SerializedAccountData, 
                                                                   connectionResult.SerializedEnviromentData, 
                                                                   connectionResult.SerializedLevelingData, 
                                                                   connectionResult.SerializedWorldData,
                                                                   connectionResult.SerializedAccountBaseParamsData,
                                                                   connectionResult.SerializedSkillSequennceData),
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
