using CoreFramework;
using System.Collections.Generic;

namespace RhytmTD.Battle.Entities.Models
{
    public class SkillsCooldownModel : BaseModel
    {
        private Dictionary<int, int> m_SkillUsageTicks = new Dictionary<int, int>();

        public void UpdateSkillUsageRecord(int skillID, int usageTick)
        {
            m_SkillUsageTicks[skillID] = usageTick;
        }

        public (int skillID, int usageTick) GetSkillUsageRecord(int skillID)
        {
            if (m_SkillUsageTicks.ContainsKey(skillID))
                return (skillID, m_SkillUsageTicks[skillID]);

            return (-1, -1);
        }
    }
}
