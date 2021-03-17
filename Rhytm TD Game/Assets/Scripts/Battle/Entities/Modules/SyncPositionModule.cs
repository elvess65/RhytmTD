
namespace RhytmTD.Battle.Entities
{
    public class SyncPositionModule : IBattleModule
    {
        public BattleEntity Target { get; }

        public SyncPositionModule(BattleEntity target)
        {
            Target = target;
        }
    }
}
