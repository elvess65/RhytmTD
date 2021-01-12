namespace RhytmTD.Data.Models.DataTableModels
{
    /// <summary>
    /// Информация об опыте и уровнях аккаунта
    /// </summary>
    public class AccountLevelingDataModel : DeserializableDataModel<AccountLevelingDataModel>
    {
        /// Используется только для парсинга данных
        public int Damage;
        public int HP;
        public int Mana;
        public LevelingProgressionConfig TestProgression;

        public override void ReorganizeData()
        {
            UnityEngine.Debug.Log($"Damage {Damage}. HP {HP}. Mana {Mana}.");
        }
    }
}
