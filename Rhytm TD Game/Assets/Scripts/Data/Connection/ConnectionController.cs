﻿using CoreFramework;
using RhytmTD.Battle.Core;

namespace RhytmTD.Data.Connection
{
    /// <summary>
    /// Контроллер подключения и получения данных
    /// </summary>
    public class ConnectionController : BaseController
    {
        public System.Action OnConnectionSuccess;
        public System.Action<int> OnConnectionError;

        private ConnectionProxy m_ConnectionProxy;


        public ConnectionController(Dispatcher dispatcher) : base(dispatcher)
        {
            m_ConnectionProxy = new ConnectionProxy();
            m_ConnectionProxy.OnConnectionSuccess += ConnectionResultSuccess;
            m_ConnectionProxy.OnConnectionError += ConnectionResultError;
        }

        public void Connect()
        {
            m_ConnectionProxy.Connect();
        }


        private void ConnectionResultSuccess(string serializedAccountData, string serializedEnviromentData, string serializedLevelingData, string serializedWorldData)
        {
            IGameSetup gameSetup = new DataGameSetup(serializedAccountData, serializedEnviromentData, serializedLevelingData, serializedWorldData);
            gameSetup.Setup();

            IGameSetup battleSetup = new BattleGameSetup();
            battleSetup.Setup();

            OnConnectionSuccess?.Invoke();
        }

        private void ConnectionResultError(int errorCode)
        {
            OnConnectionError?.Invoke(errorCode);
        }
    }
}