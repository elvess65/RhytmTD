using CoreFramework;
using RhytmTD.Battle.Entities.Skills;
using System;
using System.Collections.Generic;

namespace RhytmTD.Battle.Entities.Models
{
    public class SkillsModel : BaseModel
    {
        private Dictionary<int, BaseSkill> m_Skills = new Dictionary<int, BaseSkill>();

        public delegate void SkillUseHanlder(int skillID, int senderID, int targetID, float duration);

        public Action<BattleEntity> OnSkillCreated;
        public event SkillUseHanlder OnSkillPrepareStarted;
        public event SkillUseHanlder OnSkillUseStarted;
        public event SkillUseHanlder OnFinishingSkillUseStarted;
        public event SkillUseHanlder OnSkillUseFinished;


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


        public void SkillPrepareStarted(int skillID, int senderID, int targetID, float duration)
        {
            OnSkillPrepareStarted?.Invoke(skillID, senderID, targetID, duration);
        }

        public void SkillUseStarted(int skillID, int senderID, int targetID, float duration)
        {
            OnSkillUseStarted?.Invoke(skillID, senderID, targetID, duration);
        }

        public void FinishingSkillUseStarted(int skillID, int senderID, int targetID, float duration)
        {
            OnFinishingSkillUseStarted?.Invoke(skillID, senderID, targetID, duration);
        }

        public void SkillUseFinished(int skillID, int senderID, int targetID, float duration)
        {
            OnSkillUseFinished?.Invoke(skillID, senderID, targetID, duration);
        }
    }
}
