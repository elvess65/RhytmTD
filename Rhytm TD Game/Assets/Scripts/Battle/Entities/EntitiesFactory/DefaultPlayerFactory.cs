

using CoreFramework;
using UnityEngine;

namespace RhytmTD.Battle.Entities.EntitiesFactory
{
    [CreateAssetMenu(menuName = "BattleAssets/DefaultPlayerFactory")]
    [System.Serializable]
    public class DefaultPlayerFactory : BaseBattleEntityFactory
    {
        [SerializeField] private int MoveSpeed = 5;
        [SerializeField] private int FocusSpeed = 1;
        [SerializeField] private int Health = 100;
        [SerializeField] private int MinDamage = 4;
        [SerializeField] private int MaxDamage = 7;

        public override BattleEntity CreateEntity(Transform transform)
        {
            int entityID = IDGenerator.GenerateID();
            BattleEntity battleEntity = new BattleEntity(entityID);
            battleEntity.AddModule(new TransformModule(transform, FocusSpeed));
            battleEntity.AddModule(new MoveModule(transform, MoveSpeed));
            battleEntity.AddModule(new HealthModule(entityID, Health));
            battleEntity.AddModule(new DamageModule(MinDamage, MaxDamage));

            return battleEntity;
        }
    }
}
