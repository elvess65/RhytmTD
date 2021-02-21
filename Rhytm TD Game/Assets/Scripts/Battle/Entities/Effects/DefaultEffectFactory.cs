﻿
using CoreFramework;
using UnityEngine;

namespace RhytmTD.Battle.Entities.Effects
{
    public class DefaultEffectFactory : IEffectFactory
    {
        public BattleEntity CreateMeteoriteEffect(Vector3 position, Quaternion rotation, float moveSpeed)
        {
            int entityID = IDGenerator.GenerateID();

            BattleEntity battleEntity = new BattleEntity(entityID);
            battleEntity.AddModule(new EffectModule(1));
            battleEntity.AddModule(new TransformModule(position, rotation));
            battleEntity.AddModule(new MoveModule(moveSpeed));

            return battleEntity;
        }

        public BattleEntity CreateFireballEffect(Vector3 position, Quaternion rotation, float moveSpeed)
        {
            int entityID = IDGenerator.GenerateID();

            BattleEntity battleEntity = new BattleEntity(entityID);
            battleEntity.AddModule(new EffectModule(2));
            battleEntity.AddModule(new TransformModule(position, rotation));
            battleEntity.AddModule(new MoveModule(moveSpeed));

            return battleEntity;
        }
    }
}
