using CoreFramework;

namespace RhytmTD.Battle.Entities.Models
{
    public class StartBattleSequenceModel : BaseModel
    {
        public int AnimationDelay = 2;
        public System.Action OnSequencePrepared;
        public System.Action OnSequenceFinished;
    }
}
