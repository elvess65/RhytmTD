﻿using CoreFramework;
using UnityEngine;

namespace RhytmTD.Battle.Entities.EntitiesFactory
{
    public class DefaultEnemyFactory : BaseBattleEntityFactory
    {
        public override BattleEntity CreateEntity(Transform transform, EntityFactorySetup setup)
        {
            int entityID = IDGenerator.GenerateID();

            BattleEntity battleEntity = new BattleEntity(entityID);
            battleEntity.AddModule(new TransformModule(transform.position, transform.rotation));
            battleEntity.AddModule(new RotateModule(setup.FocusSpeed));
            battleEntity.AddModule(new FocusModule());
            battleEntity.AddModule(new HealthModule(entityID, setup.Health));
            battleEntity.AddModule(new DamageModule(setup.MinDamage, setup.MaxDamage));
            battleEntity.AddModule(new EnemyBehaviourTag());
         
            return battleEntity;
        }
    }

    public class EnemyFactorySetup : EntityFactorySetup
    {
        public int MaxHP { get; }

        public EnemyFactorySetup(float focusSpeed, int minDamage, int maxDamage, int minHP, int maxHP) : base(focusSpeed, minDamage, maxDamage, minHP)
        {
            MaxHP = maxHP;
        }
    }
}
