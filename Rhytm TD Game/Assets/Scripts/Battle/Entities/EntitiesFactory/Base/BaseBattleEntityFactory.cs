using UnityEngine;

namespace RhytmTD.Battle.Entities.EntitiesFactory
{
    public abstract class BaseBattleEntityFactory : IBattleEntityFactory
    {
        public abstract BattleEntity CreateEntity(Transform transform, EntityFactorySetup setup);
    }

    public abstract class EntityFactorySetup
    {
        public float FocusSpeed { get; }

        public int MinDamage { get; }
        public int MaxDamage { get; }

        public int Health { get; }

        public EntityFactorySetup(float focusSpeed, int minDamage, int maxDamage, int health)
        {
            FocusSpeed = focusSpeed;

            MinDamage = minDamage;
            MaxDamage = maxDamage;

            Health = health;
        }
    }
}
