using System;

namespace CoreFramework.Network
{
    /// <summary>
    /// Data base provider (simulation, GPServices, server)
    /// </summary>
    interface iDataProvider
    {
        event Action<ConnectionSuccessResult> OnConnectionSuccess;
        event Action<int> OnConnectionError;

        void Connect();
    }

    /// <summary>
    /// Local data base simulation provider
    /// </summary>
    partial class LocalDataProvider : iDataProvider
    {
        public event Action<ConnectionSuccessResult> OnConnectionSuccess;
        public event Action<int> OnConnectionError;

        private bool m_SimulateSuccessConnection;
        private DBLocal m_DataObject;


        public LocalDataProvider(bool simulateSuccessConnection)
        {
            m_DataObject = UnityEngine.Object.FindObjectOfType<DBLocal>();
            m_SimulateSuccessConnection = simulateSuccessConnection;
        }

        public void Connect()
        {
            if (m_SimulateSuccessConnection)
                SimulateSuccessConnectionDelay();
            else
                SimulateErrorConnectionDelay(100);
        }


        partial void SimulateSuccessConnectionDelay();

        partial void SimulateErrorConnectionDelay(int errorCode);
    }
}

