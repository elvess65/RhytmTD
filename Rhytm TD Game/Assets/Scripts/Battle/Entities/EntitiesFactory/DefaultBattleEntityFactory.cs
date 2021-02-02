

using UnityEngine;

namespace RhytmTD.Battle.Entities.EntitiesFactory
{
    public class DefaultBattleEntityFactory : IBattleEntityFactory // Inherit from ScriptableOBject?
    {
        public BattleEntity CreatePlayer(Transform transform)
        {
            BattleEntity battleEntity = new BattleEntity();
            battleEntity.AddNodule(new MoveModule(transform, 5));
            battleEntity.AddNodule(new HealthModule(100));
            battleEntity.AddNodule(new DamageModule(4, 7));

            return battleEntity;
        }

        public BattleEntity CreateEnemy(Transform transform)
        {
            BattleEntity battleEntity = new BattleEntity();
            battleEntity.AddNodule(new HealthModule(100));
            battleEntity.AddNodule(new DamageModule(4, 7));

            return battleEntity;
        }
    }
}
