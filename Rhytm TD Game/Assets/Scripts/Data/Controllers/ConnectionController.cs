using CoreFramework;
using RhytmTD.Battle.Core;
using RhytmTD.Core;
using RhytmTD.Data.DataBase;

namespace RhytmTD.Data.Controllers
{
    /// <summary>
    /// Контроллер подключения и получения данных
    /// </summary>
    public class ConnectionController : BaseController
    {
        public System.Action OnConnectionSuccess;
        public System.Action<int> OnConnectionError;

        private DBProxy m_DBProxy;


        public ConnectionController(Dispatcher dispatcher) : base(dispatcher)
        {
        }

        public void Connect()
        {
            m_DBProxy = new DBProxy();
            m_DBProxy.OnConnectionSuccess += ConnectionResultSuccess;
            m_DBProxy.OnConnectionError += ConnectionResultError;
            m_DBProxy.Initialize();
        }


        private void ConnectionResultSuccess(string serializedAccountData, string serializedEnviromentData, string serializedLevelingData, string serializedWorldData)
        {
            m_DBProxy = null;

            IGameSetup battleSetup = new BattleGameSetup();
            battleSetup.Setup();

            IGameSetup gameSetup = new DataGameSetup(serializedAccountData, serializedEnviromentData, serializedLevelingData, serializedWorldData);
            gameSetup.Setup();

            OnConnectionSuccess?.Invoke();
        }

        private void ConnectionResultError(int errorCode)
        {
            OnConnectionError?.Invoke(errorCode);
        }
    }
}
