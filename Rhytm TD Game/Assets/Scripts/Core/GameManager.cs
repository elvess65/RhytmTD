using CoreFramework.Abstract;
using CoreFramework.SceneLoading;
using RhytmTD.Data.DataBase;
using UnityEngine;

namespace RhytmTD.Core
{
    public class GameManager : Singleton<GameManager>
    {
        public SceneLoader SceneLoader { get; private set; }

        private DBProxy m_DBProxy;


        public void InitializeConnection()
        {
            m_DBProxy = new DBProxy();
            m_DBProxy.OnConnectionSuccess += ConnectionResultSuccess;
            m_DBProxy.OnConnectionError += ConnectionResultError;
            m_DBProxy.Initialize();
        }


        private void InitializeCore()
        {
            SceneLoader = new SceneLoader();
        }

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
            
            InitializeCore();
        }
 

        private void ConnectionResultSuccess(string serializedAccountData, string serializedEnviromentData, string serializedLevelingData, string serializedWorldData)
        {
            m_DBProxy = null;

            IGameSetup gameSetup = new DataGameSetup(serializedAccountData, serializedEnviromentData, serializedLevelingData, serializedWorldData);
            gameSetup.SetupDispatcher();
             
            SceneLoader.LoadLevel(SceneLoader.MENU_SCENE_NAME);
        }

        private void ConnectionResultError(int errorCode) => Debug.LogError($"Connection error {errorCode}");

    }
}
