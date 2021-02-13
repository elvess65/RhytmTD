using UnityEngine;

namespace RhytmTD.Battle.Entities
{
    public class DamageModule : IBattleModule
    {
        public int MinDamage;
        public int MaxDamage;

        public DamageModule()
        {
            MinDamage = 0;
            MaxDamage = 0;
        }

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
