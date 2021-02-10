namespace RhytmTD.Battle.Entities
{
    public class DamageModule : IBattleModule
    {
        public int MinDamage;
        public int MaxDamage;

        public DamageModule(int minDamage, int maxDamage)
        {
            MinDamage = minDamage;
            MaxDamage = maxDamage;
        }
    }
}
