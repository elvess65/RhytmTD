
namespace RhytmTD.Battle.Entities
{
    public class HealthSkillModule : IBattleModule
    {
        public float HealthPercent { get; }

        public HealthSkillModule(float healthPercent)
        {
            HealthPercent = healthPercent;
        }
    }
}
