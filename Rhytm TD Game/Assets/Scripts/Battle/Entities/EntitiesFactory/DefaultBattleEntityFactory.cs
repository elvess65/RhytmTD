using CoreFramework;
using UnityEngine;

namespace RhytmTD.Battle.Entities.EntitiesFactory
{
    public class DefaultBattleEntityFactory : IBattleEntityFactory
    {
        public BattleEntity CreatePlayer(int typeID, Vector3 position, Quaternion rotation, float moveSpeed, int health, int minDamage, int maxDamage, int mana)
        {
            int entityID = IDGenerator.GenerateID();

            BattleEntity battleEntity = new BattleEntity(entityID);
            battleEntity.AddModule(new TransformModule(position, rotation));
            battleEntity.AddModule(new MoveModule(moveSpeed));
            battleEntity.AddModule(new HealthModule(battleEntity, health));
            battleEntity.AddModule(new DamageModule(minDamage, maxDamage));
            battleEntity.AddModule(new DestroyModule(battleEntity));
            battleEntity.AddModule(new DamagePredictionModule());
            battleEntity.AddModule(new TargetModule());
            battleEntity.AddModule(new SlotModule());
            battleEntity.AddModule(new AnimationModule());
            battleEntity.AddModule(new LoadoutModule());
            battleEntity.AddModule(new ManaModule(mana));

            return battleEntity;
        }

        public BattleEntity CreateEnemy(int typeID, Vector3 position, Quaternion rotation, float rotateSpeed, int health, int minDamage, int maxDamage)
        {
            int entityID = IDGenerator.GenerateID();

            BattleEntity battleEntity = new BattleEntity(entityID);
            battleEntity.AddModule(new TransformModule(position, rotation));
            battleEntity.AddModule(new RotateModule(rotateSpeed));
            battleEntity.AddModule(new MoveModule());
            battleEntity.AddModule(new FocusModule());
            battleEntity.AddModule(new HealthModule(battleEntity, health));
            battleEntity.AddModule(new DestroyModule(battleEntity));
            battleEntity.AddModule(new DamageModule(minDamage, maxDamage));
            battleEntity.AddModule(new EnemyBehaviourTag());
            battleEntity.AddModule(new SlotModule());
            battleEntity.AddModule(new AnimationModule());

            return battleEntity;
        }
    }
}
