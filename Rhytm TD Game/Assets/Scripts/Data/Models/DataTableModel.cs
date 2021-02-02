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
        public WorldDataModel WorldDataModel { get; }

        public DataTableModel(string serializedEnviromentData, string serializedLevelingData, string serializedWorldData)
        {
            EnvironmentDataModel = EnvironmentDataModel.DeserializeData(serializedEnviromentData);
            LevelingDataModel = AccountLevelingDataModel.DeserializeData(serializedLevelingData);
            WorldDataModel = WorldDataModel.DeserializeData(serializedWorldData);
        }

        public void ReorganizeData()
        {
            EnvironmentDataModel.ReorganizeData();
            LevelingDataModel.ReorganizeData();
            WorldDataModel.ReorganizeData();
        }
    }
}
