namespace RhytmTD.Data.Models
{
    /// <summary>
    /// Модель содержащая данные об аккаунте
    /// </summary>
    public class AccountDataModel : DeserializableDataModel<AccountDataModel>
    {
        public int AccountExperiance;
        public int WeaponExperiance;
        public int HPExperiance;
        public int ManaExperiance;

        public int CompletedLevels;
        public int CompletedAreas;

        public override void ReorganizeData()
        {
            
        }
    }
}
//Fill
//DBSimulation_AccountData -> (is parsing to) AccountDataModel
//DBSimulation_AccountLevelingData -> (is parsing to) AccountLevelingDataModel

//Use
//AccountDataModel -> (is using) <- (returning data) AccountLevelingDataModel
