
using CoreFramework;
using UnityEngine;

namespace RhytmTD.Battle.Entities.Effects
{
    public class DefaultEffectEntityFactory : IEffectEntityFactory
    {
        public BattleEntity CreateMeteoriteEffectEntity(Vector3 position, Quaternion rotation, float moveSpeed)
        {
            int entityID = IDGenerator.GenerateID();

            BattleEntity battleEntity = new BattleEntity(entityID);
            battleEntity.AddModule(new EffectModule(EnumsCollection.BattleEntityEffectID.SkillMeterite));
            battleEntity.AddModule(new TransformModule(position, rotation));
            battleEntity.AddModule(new MoveModule(moveSpeed));

            return battleEntity;
        }

        public BattleEntity CreateFireballEffectEntity(Vector3 position, Quaternion rotation, float moveSpeed)
        {
            int entityID = IDGenerator.GenerateID();

            BattleEntity battleEntity = new BattleEntity(entityID);
            battleEntity.AddModule(new EffectModule(EnumsCollection.BattleEntityEffectID.SkillFireball));
            battleEntity.AddModule(new TransformModule(position, rotation));
            battleEntity.AddModule(new MoveModule(moveSpeed));

            return battleEntity;
        }

        public BattleEntity CreateBulletEntity(EnumsCollection.BattleEntityEffectID typeID, Vector3 position, Quaternion rotation, float speed, BattleEntity owner)
        {
            int entityID = IDGenerator.GenerateID();

            BattleEntity battleEntity = new BattleEntity(entityID);
            battleEntity.AddModule(new TransformModule(position, rotation));
            battleEntity.AddModule(new MoveModule(speed));
            battleEntity.AddModule(new TargetModule());
            battleEntity.AddModule(new DamageModule());
            battleEntity.AddModule(new OwnerModule { Owner = owner });
            battleEntity.AddModule(new DestroyModule(battleEntity));
            battleEntity.AddModule(new EffectModule(typeID));

            return battleEntity;
        }
    }
}
