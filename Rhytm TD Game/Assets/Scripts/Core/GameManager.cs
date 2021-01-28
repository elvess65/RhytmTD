using CoreFramework.Abstract;
using CoreFramework.SceneLoading;
using RhytmTD.Data;
using RhytmTD.Data.Models;
using UnityEngine;

namespace RhytmTD.Core
{
    public class GameManager : Singleton<GameManager>
    {
        public ModelsHolder ModelsHolder { get; private set; }
        public SceneLoader SceneLoader { get; private set; }


        public void InitializeConnection()
        {
            ModelsHolder.DBProxy.OnConnectionSuccess += ConnectionResultSuccess;
            ModelsHolder.DBProxy.OnConnectionError += ConnectionResultError;
            ModelsHolder.DBProxy.Initialize();
        }


        private void InitializeCore()
        {
            ModelsHolder = new ModelsHolder();
            SceneLoader = new SceneLoader();
        }

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
            
            InitializeCore();
        }
 

        private void ConnectionResultSuccess(string serializedAccountData, string serializedEnviromentData, string serializedLevelingData, string serializedWorldData)
        {
            //Set data
            ModelsHolder.AccountModel = AccountDataModel.DeserializeData(serializedAccountData);
            ModelsHolder.AccountModel.ReorganizeData();

            ModelsHolder.DataTableModel = new DataTableModel(serializedEnviromentData, serializedLevelingData, serializedWorldData);
            ModelsHolder.DataTableModel.ReorganizeData();

            SceneLoader.LoadLevel(SceneLoader.MENU_SCENE_NAME);
        }

        private void ConnectionResultError(int errorCode) => Debug.LogError($"Connection error {errorCode}");

    }
}
