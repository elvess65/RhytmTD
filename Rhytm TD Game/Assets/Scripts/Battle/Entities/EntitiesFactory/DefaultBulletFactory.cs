using CoreFramework;
using UnityEngine;

namespace RhytmTD.Battle.Entities.EntitiesFactory
{
    public class DefaultBulletFactory : BaseBattleEntityFactory
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
}
