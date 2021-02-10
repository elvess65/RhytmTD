using CoreFramework;
using RhytmTD.Data.Models;
using RhytmTD.Data.Models.DataTableModels;

namespace RhytmTD.Battle.Core
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

            ICustomSerializer serializer = new JsonSerializer();
            dispatcher.CreateModel(serializer.Deserialize<AccountDataModel>(m_SerializedAccountData));
            dispatcher.CreateModel(serializer.Deserialize<EnvironmentDataModel>(m_SerializedEnviromentData));
            dispatcher.CreateModel(serializer.Deserialize<AccountLevelingDataModel>(m_SerializedLevelingData));
            dispatcher.CreateModel(serializer.Deserialize<WorldDataModel>(m_SerializedWorldData));
        }
    }
}
