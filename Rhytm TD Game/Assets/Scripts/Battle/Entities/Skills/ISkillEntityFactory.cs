
using UnityEngine;

namespace RhytmTD.Battle.Entities.Skills
{
    public interface ISkillEntityFactory
    {
        BattleEntity CreateMeteoriteEntity(int typeID, float activationTime, float useTime, float finishingTime, int cooldownTicks,
            float damageRadius, int damage, Vector2 effectOffset);
        BattleEntity CreateFireballEntity(int typeID, float activationTime, float useTime, float finishingTime, int cooldownTicks,
            float moveSpeed, int damage);
        BattleEntity CreateHealthEntity(int typeID, float activationTime, float useTime, float finishingTime, int cooldownTicks,
            float restorePercent);
    }
}
