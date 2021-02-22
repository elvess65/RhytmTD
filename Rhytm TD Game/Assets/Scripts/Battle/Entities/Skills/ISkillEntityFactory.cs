
namespace RhytmTD.Battle.Entities.Skills
{
    public interface ISkillEntityFactory
    {
        BattleEntity CreateMeteoriteEntity();
        BattleEntity CreateFireballEntity();
    }
}
