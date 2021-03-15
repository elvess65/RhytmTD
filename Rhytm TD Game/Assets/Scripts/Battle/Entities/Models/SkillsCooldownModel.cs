using CoreFramework;
using System.Collections.Generic;

namespace RhytmTD.Battle.Entities.Models
{
    public class SkillsCooldownModel : BaseModel
    {
        public ICollection<int> SkillsInCooldownIDs => m_SkillsInCooldown.Keys;

        private Dictionary<int, float> m_SkillsInCooldown = new Dictionary<int, float>();


        public void AddSkillToCooldown(int skillID, float cooldownSeconds)
        {
            m_SkillsInCooldown[skillID] = cooldownSeconds;
        }

        public bool UpdateSkillCooldownTime(int skillID, float deltaTime)
        {
            if (m_SkillsInCooldown.ContainsKey(skillID))
            {
                m_SkillsInCooldown[skillID] -= deltaTime;

                return m_SkillsInCooldown[skillID] <= 0;
            }

            return false;
        }

        public void RemoveSkillFromCooldown(int skillID)
        {
            if (m_SkillsInCooldown.ContainsKey(skillID))
                m_SkillsInCooldown.Remove(skillID);
        }

        public float GetSkillCooldownTime(int skillID)
        {
            if (m_SkillsInCooldown.ContainsKey(skillID))
                return m_SkillsInCooldown[skillID];

            return 0;
        }
    }
}
