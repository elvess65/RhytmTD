
using UnityEngine;

namespace RhytmTD.Battle.Entities.Effects
{
    public interface IEffectEntityFactory
    {
        BattleEntity CreateMeteoriteEffectEntity(Vector3 position, Quaternion rotation, float moveSpeed);
        BattleEntity CreateFireballEffectEntity(Vector3 position, Quaternion rotation, float moveSpeed);
        BattleEntity CreateBulletEntity(int typeID, Vector3 position, Quaternion rotation, float speed, BattleEntity owner);
    }
}
