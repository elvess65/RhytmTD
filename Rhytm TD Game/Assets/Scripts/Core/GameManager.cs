﻿using System.Collections;
using CoreFramework.Abstract;
using CoreFramework.Network;
using CoreFramework.SceneLoading;
using UnityEngine;

namespace RhytmTD.Core
{
    public class GameManager : Singleton<GameManager>
    {
        public SceneLoader SceneLoader { get; private set; }

        public void InitializeConnection()
        {
            ConnectionController connectionController = new ConnectionController();
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
            StartCoroutine(StartDelay());
        }

        private void ConnectionResultError(int errorCode)
        {
            Debug.LogError($"Connection error {errorCode}");
        }

        IEnumerator StartDelay()
        {
            yield return null;

            SceneLoader.LoadLevel(SceneLoader.MENU_SCENE_NAME);
        }
    }
}
