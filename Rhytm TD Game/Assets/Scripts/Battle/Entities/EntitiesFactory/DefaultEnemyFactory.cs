using CoreFramework;
using UnityEngine;

namespace RhytmTD.Battle.Entities.EntitiesFactory
{
    [CreateAssetMenu(menuName = "BattleAssets/DefaultEnemyFactory")]
    [System.Serializable]
    public class DefaultEnemyFactory : BaseBattleEntityFactory
    {
        [SerializeField] private int Health = 20;
        [SerializeField] private int MinDamage = 4;
        [SerializeField] private int MaxDamage = 7;

        public override BattleEntity CreateEntity(Transform transform)
        {
            int entityID = IDGenerator.GenerateID();
            BattleEntity battleEntity = new BattleEntity(entityID);
            battleEntity.AddModule(new TransformModule(transform));
            battleEntity.AddModule(new HealthModule(entityID, Health));
            battleEntity.AddModule(new DamageModule(MinDamage, MaxDamage));

            return battleEntity;
        }
    }
}
