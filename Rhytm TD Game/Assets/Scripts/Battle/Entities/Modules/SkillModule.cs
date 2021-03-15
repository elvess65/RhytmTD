
namespace RhytmTD.Battle.Entities
{
    public class SkillModule : IBattleModule
    {
        public int TypeID { get; }
        public float ActivationTime { get; }
        public float UseTime { get; }
        public float FinishingTime { get; }
        public int CooldownTicks { get; }

        public delegate void SkillUseHanlder(int senderID);

        public event SkillUseHanlder OnSkillPrepareStarted;
        public event SkillUseHanlder OnSkillUseStarted;
        public event SkillUseHanlder OnFinishingSkillUseStarted;
        public event SkillUseHanlder OnSkillUseFinished;

        public SkillModule(int typeID, float activationTime, float useTime, float finishingTime, int cooldownTicks)
        {
            TypeID = typeID;
            ActivationTime = activationTime;
            UseTime = useTime;
            FinishingTime = finishingTime;
            CooldownTicks = cooldownTicks;
        }

        public void SkillPrepareStarted(int senderID)
        {
            OnSkillPrepareStarted?.Invoke(senderID);
        }

        public void SkillUseStarted(int senderID)
        {
            OnSkillUseStarted?.Invoke(senderID);
        }

        public void FinishingSkillUseStarted(int senderID)
        {
            OnFinishingSkillUseStarted?.Invoke(senderID);
        }

        public void SkillUseFinished(int senderID)
        {
            OnSkillUseFinished?.Invoke(senderID);
        }
    }
}
