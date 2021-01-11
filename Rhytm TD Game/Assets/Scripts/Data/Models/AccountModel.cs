namespace RhytmTD.Data.Models
{
    /// <summary>
    /// Модель содержащая данные об аккаунте
    /// </summary>
    public class AccountModel : DeserializableDataModel<AccountModel>
    {
        public int WeaponExperiance;
        public int HPExperiance;
        public int ManaExperiance;

        /// <summary>
        /// Используется только для парсинга данных
        /// </summary>
        public override void ReorganizeData()
        {
            
        }
    }
}
