using CoreFramework;
using CoreFramework.Rhytm;
using RhytmTD.Battle.Entities.Models;

namespace RhytmTD.Battle.Entities.Controllers
{
    public class SkillsCooldownController : BaseController
    {
        private SkillsModel m_SkillsModel;
        private SkillsCooldownModel m_SkillsCooldownModel;
        private RhytmController m_RhytmController;
      

        public SkillsCooldownController(Dispatcher dispatcher) : base(dispatcher)
        {     
        }

        public override void InitializeComplete()
        {
            base.InitializeComplete();

            m_SkillsModel = Dispatcher.GetModel<SkillsModel>();
            m_SkillsCooldownModel = Dispatcher.GetModel<SkillsCooldownModel>();
            m_RhytmController = Dispatcher.GetController<RhytmController>();

            m_SkillsModel.OnSkillUsed += SkillUsedHandler;
        }

        public bool IsSkillInCooldown(int skillID, out float remainsCooldownSeconds)
        {
            remainsCooldownSeconds = 0;
            (int skillID, int usageTick) result = m_SkillsCooldownModel.GetSkillUsageRecord(skillID);
            UnityEngine.Debug.Log("GET RECORD FOR " + skillID + " " + result.usageTick);
            if (result.usageTick >= 0)
            {
                int cooldownFinishTick = result.usageTick + m_SkillsModel.GetSkill(skillID).BattleEntity.GetModule<SkillModule>().CooldownTicks;
                int ticksInCooldown = cooldownFinishTick - m_RhytmController.CurrentTick;
                if (ticksInCooldown > 0)
                {
                    remainsCooldownSeconds = (float)(ticksInCooldown * m_RhytmController.TickDurationSeconds);
                    return true;
                }
            }

            return false;
        }


        private void SkillUsedHandler(int skillID)
        {
            UnityEngine.Debug.Log("Skill used: " + skillID);
        }
    }
}
