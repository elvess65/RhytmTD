
using UnityEngine;

namespace RhytmTD.Battle.Entities.Skills
{
    public interface ISkillEntityFactory
    {
        BattleEntity CreateMeteoriteEntity(int typeID, float activationTime, float useTime, float finishingTime, float cooldownTime,
            float flyTime, float damageRadius, int damage, Vector2 effectOffset);
        BattleEntity CreateFireballEntity(int typeID, float activationTime, float useTime, float finishingTime, float cooldownTime,
            float moveSpeed, int damage);
    }
}
