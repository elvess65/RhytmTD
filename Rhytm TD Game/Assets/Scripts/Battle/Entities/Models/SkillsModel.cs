using CoreFramework;
using RhytmTD.Battle.Entities.Skills;
using System;
using System.Collections.Generic;

namespace RhytmTD.Battle.Entities.Models
{
    public class SkillsModel : BaseModel
    {
        private Dictionary<int, BaseSkill> m_Skills = new Dictionary<int, BaseSkill>();

        public Action<BattleEntity> OnSkillCreated;
        public Action<int, int> OnPrepareSkill;

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
    }
}
