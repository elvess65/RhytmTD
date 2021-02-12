
using UnityEngine;

namespace RhytmTD.Battle.Entities.EntitiesFactory
{
    public interface IBattleEntityFactory
    {
        BattleEntity CreatePlayer(int typeID, Vector3 position, Quaternion rotation, float moveSpeed, int health, int minDamage, int maxDamage);
        BattleEntity CreateEnemy(int typeID, Vector3 position, Quaternion rotation, float rotateSpeed, int health, int minDamage, int maxDamage);
        BattleEntity CreateBullet(int typeID, Vector3 position, Quaternion rotation, float speed, BattleEntity owner);
    }
}