using System.Collections.Generic;

namespace RhytmTD.Battle.Entities
{
    /// <summary>
    /// Holds entity related stuff as skills, weapons, etc.
    /// </summary>
    public class LoadoutModule : IBattleModule
    {
        public ICollection<int> SelectedSkillIDs => m_SelectedSkills.Values;

        private Dictionary<int, int> m_SelectedSkills = new Dictionary<int, int>();

        /// <summary>
        /// Holds entity related stuff as skills, weapons, etc.
        /// </summary>
        public LoadoutModule()
        {

        }

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
