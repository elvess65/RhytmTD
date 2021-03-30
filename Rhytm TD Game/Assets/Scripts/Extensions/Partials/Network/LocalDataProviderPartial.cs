using UnityEngine;

namespace CoreFramework.Network
{
    partial class LocalDataProvider
    {
        partial void SimulateSuccessConnectionDelay()
        {
            OnConnectionSuccess?.Invoke(new ConnectionSuccessResult(JsonUtility.ToJson(m_DataObject.AccountData),
                                                                    JsonUtility.ToJson(m_DataObject.EnvironmentData),
                                                                    JsonUtility.ToJson(m_DataObject.AccountLevelingData),
                                                                    JsonUtility.ToJson(m_DataObject.WorldData),
                                                                    JsonUtility.ToJson(m_DataObject.AccountBaseParamsData),
                                                                    JsonUtility.ToJson(m_DataObject.SkillSequenceData)));
        }


        partial void SimulateErrorConnectionDelay(int errorCode)
        {
            OnConnectionError?.Invoke(errorCode);
        }
    }
}
