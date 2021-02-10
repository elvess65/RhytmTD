using UnityEngine;

namespace RhytmTD.Data.DataBaseLocal
{
    [CreateAssetMenu(fileName = "New Local AccountData", menuName = "DBLocal/Account/AccountData", order = 101)]
    public class DBLocal_AccountData : ScriptableObject
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

        [Tooltip("Количество открытых зон")]
        public int CompletedAreas;
    }
}
