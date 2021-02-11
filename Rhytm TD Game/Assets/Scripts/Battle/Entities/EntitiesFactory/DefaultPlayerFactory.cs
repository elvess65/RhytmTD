using CoreFramework;
using UnityEngine;

namespace RhytmTD.Battle.Entities.EntitiesFactory
{
    public class DefaultPlayerFactory : BaseBattleEntityFactory
    {
        public override BattleEntity CreateEntity(Transform transform, EntityFactorySetup setup)
        {
            int entityID = IDGenerator.GenerateID();

            BattleEntity battleEntity = new BattleEntity(entityID);
            battleEntity.AddModule(new TransformModule(transform.position, transform.rotation));
            battleEntity.AddModule(new MoveModule((setup as PlayerFactorySetup).MoveSpeed));
            battleEntity.AddModule(new HealthModule(entityID, setup.Health));
            battleEntity.AddModule(new DamageModule(setup.MinDamage, setup.MaxDamage));

            return battleEntity;
        }
    }

    public class PlayerFactorySetup : EntityFactorySetup
    {
        public float MoveSpeed { get; }
        public int Mana { get; }

        public PlayerFactorySetup(float focusSpeed, int minDamage, int maxDamage, int health, float moveSpeed, int mana) : base(focusSpeed, minDamage, maxDamage, health)
        {
            MoveSpeed = moveSpeed;
            Mana = mana;
        }
    }
}
