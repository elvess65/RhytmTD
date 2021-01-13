using UnityEngine;

namespace RhytmTD.Data.DataBase.Simulation
{
    /// <summary>
    /// Данные по улучшению параметров аккаунта
    /// </summary>
    [System.Serializable]
    [CreateAssetMenu(fileName = "New Simulation AccountLevelingData", menuName = "DBSimulation/Account/AccountLevelingData", order = 101)]
    public class DBSimulation_AccountLevelingData : ScriptableObject
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
