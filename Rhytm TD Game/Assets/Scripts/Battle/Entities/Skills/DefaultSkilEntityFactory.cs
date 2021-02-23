﻿
using CoreFramework;
using UnityEngine;

namespace RhytmTD.Battle.Entities.Skills
{
    public class DefaultSkilEntityFactory : ISkillEntityFactory
    {
        public BattleEntity CreateMeteoriteEntity(int typeID, float activationTime, float useTime, float finishingTime, float cooldownTime,
                                                  float flyTime, float damageRadius, int damage, Vector2 effectOffset)
        {
            int entityID = IDGenerator.GenerateID();

            BattleEntity battleEntity = new BattleEntity(entityID);
            battleEntity.AddModule(new SkillModule(typeID, activationTime, useTime, finishingTime, cooldownTime));
            battleEntity.AddModule(new MeteoriteSkillModule(flyTime, damageRadius, damage, effectOffset));

            return battleEntity;
        }

        public BattleEntity CreateFireballEntity(int typeID, float activationTime, float useTime, float finishingTime, float cooldownTime,
                                                 float moveSpeed, int damage)
        {
            int entityID = IDGenerator.GenerateID();

            BattleEntity battleEntity = new BattleEntity(entityID);
            battleEntity.AddModule(new SkillModule(typeID, activationTime, useTime, finishingTime, cooldownTime));
            battleEntity.AddModule(new FireballSkillModule(moveSpeed, damage));

            return battleEntity;
        }
    }
}