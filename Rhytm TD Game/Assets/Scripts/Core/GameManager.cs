using CoreFramework;
using CoreFramework.Abstract;
using CoreFramework.SceneLoading;
using RhytmTD.Data.Connection;
using UnityEngine;

namespace RhytmTD.Core
{
    public class GameManager : Singleton<GameManager>
    {
        public SceneLoader SceneLoader { get; private set; }


        public void InitializeConnection()
        {
            IGameSetup connectionSetup = new ConnectionGameSetup();
            connectionSetup.Setup();

            ConnectionController connectionController = Dispatcher.Instance.GetController<ConnectionController>();
            connectionController.OnConnectionSuccess += ConnectionResultSuccess;
            connectionController.OnConnectionError += ConnectionResultError;
            connectionController.Connect();
        }


        private void Start()
        {
            DontDestroyOnLoad(gameObject);

            SceneLoader = new SceneLoader();
        }
 
        private void ConnectionResultSuccess()
        {
            SceneLoader.LoadLevel(SceneLoader.MENU_SCENE_NAME);
        }

        private void ConnectionResultError(int errorCode)
        {
            Debug.LogError($"Connection error {errorCode}");
        }
    }
}
