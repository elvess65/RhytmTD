using CoreFramework;
using System.Collections.Generic;

namespace RhytmTD.Battle.Entities.Models
{
    public class SkillsCooldownModel : BaseModel
    {
        private List<SkillCooldownData> m_SkillsInCooldownList = new List<SkillCooldownData>();


        public void AddSkillToCooldown(int skillID, float cooldownSeconds)
        {
            if (!IsSkillInCooldown(skillID, out SkillCooldownData data))
            {
                m_SkillsInCooldownList.Add(new SkillCooldownData(skillID, cooldownSeconds));
            }
        }

        public void UpdateSkillsCooldownTime(float deltaTime)
        {
            for (int i = 0; i < m_SkillsInCooldownList.Count; i++)
            {
                m_SkillsInCooldownList[i].CooldownRemainTime -= deltaTime;

                if (m_SkillsInCooldownList[i].CooldownRemainTime <= 0)
                {
                    m_SkillsInCooldownList.RemoveAt(i);
                    i--;
                }
            }
        }

        public (float remainTime, float totalTime) GetSkillCooldownTime(int skillID)
        {
            if (IsSkillInCooldown(skillID, out SkillCooldownData data))
                return (data.CooldownRemainTime, data.CooldownTotalTime);

            return (0, 0);
        }


        private bool IsSkillInCooldown(int skillID, out SkillCooldownData data)
        {
            data = null;
            foreach (SkillCooldownData skillCooldownData in m_SkillsInCooldownList)
            {
                if (skillCooldownData.SkillID == skillID)
                {
                    data = skillCooldownData;
                    return true;
                }
            }

            return false;
        }


        private class SkillCooldownData
        {
            public int SkillID { get; private set; }
            public float CooldownTotalTime { get; private set; }
            public float CooldownRemainTime { get; set; }

            public SkillCooldownData(int skillID, float cooldownTotalTime)
            {
                SkillID = skillID;
                CooldownTotalTime = cooldownTotalTime;
                CooldownRemainTime = CooldownTotalTime;
            }
        }

    }
}
