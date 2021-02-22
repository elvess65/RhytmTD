
using CoreFramework;
using UnityEngine;

namespace RhytmTD.Battle.Entities.Skills
{
    public class DefaultSkilEntityFactory : ISkillEntityFactory
    {
        public BattleEntity CreateMeteoriteEntity()
        {
            int entityID = IDGenerator.GenerateID();

            BattleEntity battleEntity = new BattleEntity(entityID);
            battleEntity.AddModule(new SkillModule(1, 1, 1, 1, 10));
            battleEntity.AddModule(new MeteoriteSkillModule(1, 5, 10, new Vector3(7.7f, 7.7f, 0)));

            return battleEntity;
        }

        public BattleEntity CreateFireballEntity()
        {
            int entityID = IDGenerator.GenerateID();

            BattleEntity battleEntity = new BattleEntity(entityID);
            battleEntity.AddModule(new SkillModule(1, 1, 1, 1, 10));
            battleEntity.AddModule(new FireballSkillModule(4, 20));

            return battleEntity;
        }
    }
}
