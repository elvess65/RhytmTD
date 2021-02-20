using System.Collections.Generic;

namespace RhytmTD.Battle.Entities
{
    public class LoadoutModule : IBattleModule
    {
        private Dictionary<int, int> m_SelectedSkills = new Dictionary<int, int>();

        public void AddSkill(int typeID, int skillID)
        {
            m_SelectedSkills.Add(typeID, skillID);
        }

        public void RemoveSkill(int typeID)
        {
            m_SelectedSkills.Remove(typeID);
        }

        public int GetSkillIDByTypeID(int skillTypeID)
        {
            return m_SelectedSkills[skillTypeID];
        }
    }
}
