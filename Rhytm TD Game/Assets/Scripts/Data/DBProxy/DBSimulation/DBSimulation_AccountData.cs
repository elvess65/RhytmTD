using UnityEngine;

namespace RhytmTD.Data.DataBase.Simulation
{
    [CreateAssetMenu(fileName = "New Simulation AccountData", menuName = "DBSimulation/Account/AccountData", order = 101)]
    public class DBSimulation_AccountData : ScriptableObject
    {
        [Header("Данные аккаунта")]

        [Tooltip("Опыт аккаунта")]
        public int AccountExperiance;

        [Tooltip("Опыт оружия")]
        public int WeaponExperiance;

        [Tooltip("Опыт ХР")]
        public int HPExperiance;

        [Tooltip("Опыт маны")]
        public int ManaExperiance;

        [Tooltip("Количество пройденных уровней в последней зоне")]
        public int CompletedLevels;

        [Tooltip("ID открытых зон")]
        public int[] CompletedAreas;
    }
}
