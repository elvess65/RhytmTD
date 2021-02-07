

using UnityEngine;

namespace RhytmTD.Battle.Entities.EntitiesFactory
{
    public abstract class BaseBattleEntityFactory : ScriptableObject, IBattleEntityFactory
    {
        public abstract BattleEntity CreateEntity(Transform transform, float progression01);
    }
}
