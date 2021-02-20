
namespace RhytmTD.Battle.Entities
{
    public class MeteoriteModule : IBattleModule
    {
        public float FlyTime { get; }
        public float DamageRadius { get; }
        public int Damage { get; }

        public MeteoriteModule(float flyTime, float damageRadius, int damage)
        {
            FlyTime = flyTime;
            DamageRadius = damageRadius;
            Damage = damage;
        }
    }
}
