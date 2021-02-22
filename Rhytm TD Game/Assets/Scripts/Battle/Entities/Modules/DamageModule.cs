using UnityEngine;

namespace RhytmTD.Battle.Entities
{
    /// <summary>
    /// Holds min/max damage of the entity
    /// </summary>
    public class DamageModule : IBattleModule
    {
        public int MinDamage;
        public int MaxDamage;

        /// <summary>
        /// Holds min/max damage of the entity
        /// </summary>
        public DamageModule()
        {
            MinDamage = 0;
            MaxDamage = 0;
        }

        /// <summary>
        /// Holds min/max damage of the entity
        /// </summary>
        public DamageModule(int minDamage, int maxDamage)
        {
            MinDamage = minDamage;
            MaxDamage = maxDamage;
        }

        public int RandomDamage()
        {
            return Random.Range(MinDamage, MaxDamage);
        }
    }
}
