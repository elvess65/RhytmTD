

using UnityEngine;

namespace RhytmTD.Battle.Entities.EntitiesFactory
{
    public interface IBattleEntityFactory
    {
        BattleEntity CreatePlayer(Transform transform);
        BattleEntity CreateEnemy(Transform transform);
    }
}
