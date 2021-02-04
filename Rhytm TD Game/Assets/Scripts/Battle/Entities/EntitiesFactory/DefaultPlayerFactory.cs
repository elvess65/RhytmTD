

using CoreFramework;
using UnityEngine;

namespace RhytmTD.Battle.Entities.EntitiesFactory
{
    [CreateAssetMenu(menuName = "BattleAssets/DefaultPlayerFactory")]
    [System.Serializable]
    public class DefaultPlayerFactory : BaseBattleEntityFactory
    {
        [SerializeField] private int MoveSpeed = 5;
        [SerializeField] private int Health = 100;
        [SerializeField] private int MinDamage = 4;
        [SerializeField] private int MaxDamage = 7;

        public override BattleEntity CreateEntity(Transform transform)
        {
            BattleEntity battleEntity = new BattleEntity(IDGenerator.GenerateID());
            battleEntity.AddModule(new MoveModule(transform, MoveSpeed));
            battleEntity.AddModule(new HealthModule(Health));
            battleEntity.AddModule(new DamageModule(MinDamage, MaxDamage));

            return battleEntity;
        }
    }
}
