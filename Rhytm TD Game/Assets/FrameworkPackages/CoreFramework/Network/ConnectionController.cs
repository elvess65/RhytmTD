namespace CoreFramework.Network
{
    /// <summary>
    /// Контроллер подключения и получения данных
    /// </summary>
    public partial class ConnectionController
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


        partial void Setup(ConnectionSuccessResult connectionResult);

        private void ConnectionResultSuccess(ConnectionSuccessResult connectionResult)
        {
            Setup(connectionResult);

            OnConnectionSuccess?.Invoke();
        }

        private void ConnectionResultError(int errorCode)
        {
            OnConnectionError?.Invoke(errorCode);
        }
    }
}
