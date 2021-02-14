using UnityEngine;

namespace RhytmTD.Data.DataBaseLocal
{
    /// <summary>
    /// Базовые параметры аккаунта (без прокачки)
    /// </summary>
    [System.Serializable]
    [CreateAssetMenu(fileName = "New Local AccountBaseParamsData", menuName = "DBLocal/Account/AccountBaseParamsData", order = 101)]
    public class DBLocal_AccountBaseParamsData : ScriptableObject
    {
        public float MoveSpeedUnitsPerTick;
        public float FocusSpeed;
        public int MinDamage;
        public int MaxDamage;
        public int Health;
        public int Mana;
    }
}
