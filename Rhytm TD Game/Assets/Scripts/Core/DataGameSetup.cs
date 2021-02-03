using CoreFramework;
using RhytmTD.Data.Models;
using RhytmTD.Data.Models.DataTableModels;

namespace RhytmTD.Core
{
    /// <summary>
    /// Setup требующий заполнения из БД
    /// </summary>
    public class DataGameSetup : IGameSetup
    {
        private string m_SerializedAccountData;
        private string m_SerializedEnviromentData;
        private string m_SerializedLevelingData;
        private string m_SerializedWorldData;

        public DataGameSetup(string serializedAccountData, string serializedEnviromentData, string serializedLevelingData, string serializedWorldData)
        {
            m_SerializedAccountData = serializedAccountData;
            m_SerializedEnviromentData = serializedEnviromentData;
            m_SerializedLevelingData = serializedLevelingData;
            m_SerializedWorldData = serializedWorldData;
        }

        public void Setup()
        {
            Dispatcher dispatcher = Dispatcher.Instance;

            // Models
            dispatcher.CreateModelFromJson<AccountDataModel>(m_SerializedAccountData);
            dispatcher.CreateModelFromJson<EnvironmentDataModel>(m_SerializedEnviromentData);
            dispatcher.CreateModelFromJson<AccountLevelingDataModel>(m_SerializedLevelingData);
            dispatcher.CreateModelFromJson<WorldDataModel>(m_SerializedWorldData);
        }
    }
}
