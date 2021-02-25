
using UnityEngine;

namespace RhytmTD.Battle.Entities
{
    /// <summary>
    /// Holds data about meteorite skill
    /// </summary>
    public class MeteoriteSkillModule : IBattleModule
    {
        public float DamageRadius { get; }
        public int Damage { get; }
        public Vector3 EffectOffset { get; }

        /// <summary>
        /// Holds data for meteorite skill
        /// </summary>
        public MeteoriteSkillModule(float damageRadius, int damage, Vector2 effectOffset)
        {
            DamageRadius = damageRadius;
            Damage = damage;
            EffectOffset = effectOffset;
        }
    }
}
