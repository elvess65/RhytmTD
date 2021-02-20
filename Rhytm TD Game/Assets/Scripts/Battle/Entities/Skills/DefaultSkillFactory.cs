
using CoreFramework;

namespace RhytmTD.Battle.Entities.Skills
{
    public class DefaultSkillFactory : ISkillFactory
    {
        public BattleEntity CreateMeteorite()
        {
            int entityID = IDGenerator.GenerateID();

            BattleEntity battleEntity = new BattleEntity(entityID);
            battleEntity.AddModule(new SkillModule(1, 1, 1, 1, 10));
            battleEntity.AddModule(new MeteoriteModule(2, 50, 10));

            return battleEntity;
        }
    }
}
