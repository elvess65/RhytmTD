using RhytmTD.Core;
using System;
using System.Collections;
using UnityEngine;

namespace CoreFramework.Network
{
    /// <summary>
    /// Data base provider (simulation, GPServices, server)
    /// </summary>
    interface iDataProvider
    {
        event Action<ConnectionSeccessResult> OnConnectionSuccess;
        event Action<int> OnConnectionError;

        void Connect();
    }

    /// <summary>
    /// Local data base simulation provider
    /// </summary>
    class LocalDataProvider : iDataProvider
    {
        public event Action<ConnectionSeccessResult> OnConnectionSuccess;
        public event Action<int> OnConnectionError;

        private bool m_SimulateSuccessConnection;
        private DBLocal m_DataObject;
        private WaitForSeconds m_WaitConnectionDelay;

        private const float m_CONNECTION_TIME = 1;


        public LocalDataProvider(bool simulateSuccessConnection)
        {
            m_DataObject = UnityEngine.Object.FindObjectOfType<DBLocal>();
            m_WaitConnectionDelay = new WaitForSeconds(m_CONNECTION_TIME);
            m_SimulateSuccessConnection = simulateSuccessConnection;
        }

        public void Connect()
        {
            Debug.Log("Start connection");

            if (m_SimulateSuccessConnection)
                GameManager.Instance.StartCoroutine(SimulateSuccessConnectionDelay());
            else
                GameManager.Instance.StartCoroutine(SimulateErrorConnectionDelay(100));
        }


        IEnumerator SimulateSuccessConnectionDelay()
        {
            yield return m_WaitConnectionDelay;

            OnConnectionSuccess?.Invoke(new ConnectionSeccessResult(JsonUtility.ToJson(m_DataObject.AccountData),
                                                                    JsonUtility.ToJson(m_DataObject.EnvironmentData),
                                                                    JsonUtility.ToJson(m_DataObject.AccountLevelingData),
                                                                    JsonUtility.ToJson(m_DataObject.WorldData),
                                                                    JsonUtility.ToJson(m_DataObject.AccountBaseParamsData),
                                                                    JsonUtility.ToJson(m_DataObject.SkillSequenceData)));
        }
        

        IEnumerator SimulateErrorConnectionDelay(int errorCode)
        {
            yield return m_WaitConnectionDelay;

            OnConnectionError?.Invoke(errorCode);
        }
    }
}

