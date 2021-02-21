
using UnityEngine;

namespace RhytmTD.Battle.Entities
{
    public class MeteoriteModule : IBattleModule
    {
        public float FlyTime { get; }
        public float DamageRadius { get; }
        public int Damage { get; }
        public Vector3 EffectOffset { get; }

        public MeteoriteModule(float flyTime, float damageRadius, int damage, Vector3 effectOffset)
        {
            FlyTime = flyTime;
            DamageRadius = damageRadius;
            Damage = damage;
            EffectOffset = effectOffset;
        }
    }
}
