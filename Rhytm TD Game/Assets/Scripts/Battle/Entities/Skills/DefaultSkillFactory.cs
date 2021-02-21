
using CoreFramework;
using UnityEngine;

namespace RhytmTD.Battle.Entities.Skills
{
    public class DefaultSkillFactory : ISkillFactory
    {
        public BattleEntity CreateMeteorite()
        {
            int entityID = IDGenerator.GenerateID();

            BattleEntity battleEntity = new BattleEntity(entityID);
            battleEntity.AddModule(new SkillModule(1, 1, 1, 1, 10));
            battleEntity.AddModule(new MeteoriteModule(1, 5, 10, new Vector3(7.7f, 7.7f, 0)));

            return battleEntity;
        }

        public BattleEntity CreateFireball()
        {
            int entityID = IDGenerator.GenerateID();

            BattleEntity battleEntity = new BattleEntity(entityID);
            battleEntity.AddModule(new SkillModule(1, 1, 1, 1, 10));
            battleEntity.AddModule(new FireballModule(4, 20));

            return battleEntity;
        }
    }
}
