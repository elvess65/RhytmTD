
namespace RhytmTD.Battle.Entities
{
    public class SkillModule : IBattleModule
    {
        public int TypeID { get; }
        public float ActivationTime { get; }
        public float UseTime { get; }
        public float FinishingTime { get; }
        public float CooldownTime { get; }

        public SkillModule(int typeID, float activationTime, float useTime, float finishingTime, float cooldownTime)
        {
            TypeID = typeID;
            ActivationTime = activationTime;
            UseTime = useTime;
            FinishingTime = finishingTime;
            CooldownTime = cooldownTime;
        }
    }
}
