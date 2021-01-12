using RhytmTD.Data.Models.DataTableModels;

namespace RhytmTD.Data.Models
{
    /// <summary>
    /// Модель содержащая информационные таблицы и конфигурации
    /// </summary>
    public class DataTableModel 
    {
        public EnvironmentDataModel EnvironmentDataModel { get; }
        public AccountLevelingDataModel LevelingDataModel { get; }

        public DataTableModel(string serializedEnviromentData, string serializedLevelingData)
        {
            EnvironmentDataModel = EnvironmentDataModel.DeserializeData(serializedEnviromentData);
            LevelingDataModel = AccountLevelingDataModel.DeserializeData(serializedLevelingData);
        }

        public void ReorganizeData()
        {
            EnvironmentDataModel.ReorganizeData();
            LevelingDataModel.ReorganizeData();
        }
    }
}
