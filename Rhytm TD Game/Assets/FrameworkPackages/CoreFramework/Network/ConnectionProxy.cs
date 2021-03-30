namespace CoreFramework.Network
{
    /// <summary>
    /// Data base connection proxy
    /// </summary>
    public class ConnectionProxy
    {
        public System.Action<ConnectionSuccessResult> OnConnectionSuccess;
        public System.Action<int> OnConnectionError;

        private iDataProvider m_DataProvider;

        
        public void Connect()
        {
            bool useSimulation = true;
            if (useSimulation)
            {
                bool simulateError = false;
                m_DataProvider = new LocalDataProvider(!simulateError);
            }
            else
            {
                UnityEngine.Debug.Log("Connecting to remote IP");
                OnConnectionErrorHandler(1);
            }

            m_DataProvider.OnConnectionSuccess += ConnectionSuccessHandler;
            m_DataProvider.OnConnectionError += OnConnectionErrorHandler;
            m_DataProvider.Connect();
        }


        private void ConnectionSuccessHandler(ConnectionSuccessResult connectionResult)
        {
            OnConnectionSuccess?.Invoke(connectionResult);
        }

        private void OnConnectionErrorHandler(int errorCode)
        {
            OnConnectionError?.Invoke(errorCode);
        }
    }
}
