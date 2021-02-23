
using UnityEngine;

namespace RhytmTD.Battle.Entities
{
    /// <summary>
    /// Holds data about meteorite skill
    /// </summary>
    public class MeteoriteSkillModule : IBattleModule
    {
        public float FlyTime { get; }
        public float DamageRadius { get; }
        public int Damage { get; }
        public Vector3 EffectOffset { get; }

        /// <summary>
        /// Holds data for meteorite skill
        /// </summary>
        public MeteoriteSkillModule(float flyTime, float damageRadius, int damage, Vector2 effectOffset)
        {
            FlyTime = flyTime;
            DamageRadius = damageRadius;
            Damage = damage;
            EffectOffset = effectOffset;
        }
    }
}
