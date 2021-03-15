
namespace RhytmTD.Battle.Entities
{
    public class SkillModule : IBattleModule
    {
        public int TypeID { get; }
        public float ActivationTime { get; }
        public float UseTime { get; }
        public float FinishingTime { get; }
        public int CooldownTicks { get; }

        public delegate void SkillUseHanlder(int senderID, int targetID);

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

        public void SkillPrepareStarted(int senderID, int targetID)
        {
            OnSkillPrepareStarted?.Invoke(senderID, targetID);
        }

        public void SkillUseStarted(int senderID, int targetID)
        {
            OnSkillUseStarted?.Invoke(senderID, targetID);
        }

        public void FinishingSkillUseStarted(int senderID, int targetID)
        {
            OnFinishingSkillUseStarted?.Invoke(senderID, targetID);
        }

        public void SkillUseFinished(int senderID, int targetID)
        {
            OnSkillUseFinished?.Invoke(senderID, targetID);
        }
    }
}
