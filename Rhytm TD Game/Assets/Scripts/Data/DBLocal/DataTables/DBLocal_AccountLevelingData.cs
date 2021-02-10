using UnityEngine;

namespace RhytmTD.Data.DataBaseLocal
{
    /// <summary>
    /// Данные по улучшению параметров аккаунта
    /// </summary>
    [System.Serializable]
    [CreateAssetMenu(fileName = "New Local AccountLevelingData", menuName = "DBLocal/Account/AccountLevelingData", order = 101)]
    public class DBLocal_AccountLevelingData : ScriptableObject
    {
        public int Damage;
        public int HP;
        public int Mana;

        public LevelingProgressionConfig TestLevelProgression;
        public ProgressionConfig TestPriceProgression;
        public ProgressionConfig TestValueProgression;
        public MinMaxProgressionConfig TestMinMaxProgression;
    }
}
