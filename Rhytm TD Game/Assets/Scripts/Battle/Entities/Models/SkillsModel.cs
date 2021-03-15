using CoreFramework;
using RhytmTD.Battle.Entities.Skills;
using System;
using System.Collections.Generic;

namespace RhytmTD.Battle.Entities.Models
{
    public class SkillsModel : BaseModel
    {
        private Dictionary<int, int> m_SkillUsageTicks = new Dictionary<int, int>();
        private Dictionary<int, BaseSkill> m_Skills = new Dictionary<int, BaseSkill>();

        public Action<BattleEntity> OnSkillCreated;

        public void AddSkill(BaseSkill skill)
        {
            m_Skills.Add(skill.ID, skill);
        }

        public void RemoveSkill(int skillID)
        {
            m_Skills.Remove(skillID);
        }

        public BaseSkill GetSkill(int skillID)
        {
            return m_Skills[skillID];
        }


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
