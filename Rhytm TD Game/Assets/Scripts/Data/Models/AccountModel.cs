namespace RhytmTD.Data.Models
{
    /// <summary>
    /// Модель содержащая данные об аккаунте
    /// </summary>
    public class AccountModel : DeserializableDataModel<AccountModel>
    {
        public int AccountExperiance;
        public int WeaponExperiance;
        public int HPExperiance;
        public int ManaExperiance;

        public override void ReorganizeData()
        {
            
        }
    }
}

//Account
//DBSimulation_AccountData -> (is parsing to) AccountModel
//DBSimulation_AccountLevelingData -> (is parsing to) AccountLevelingDataModel
