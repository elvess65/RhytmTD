

using UnityEngine;

namespace RhytmTD.Battle.Entities.EntitiesFactory
{
    public interface IBattleEntityFactory
    {
        BattleEntity CreateEntity(Transform transform);
    }
}
