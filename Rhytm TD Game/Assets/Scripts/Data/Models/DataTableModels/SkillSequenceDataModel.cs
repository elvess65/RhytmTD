using CoreFramework;
using System.Collections.Generic;

namespace RhytmTD.Data.Models.DataTableModels
{
    /// <summary>
    /// Паттерны повторения скиллов
    /// </summary>
    public class SkillSequenceDataModel : BaseModel
    {
        public List<SequencePatternData> SkillSequencePatterns;

        private Dictionary<EnumsCollection.SkillSequencePatternID, List<bool>> m_SkillSequencePatterns;

        public List<bool> GetSkillSequencePatternByID(EnumsCollection.SkillSequencePatternID patternID)
        {
            return m_SkillSequencePatterns[patternID];
        }

        public override  void Initialize()
        {
            base.Initialize();

            m_SkillSequencePatterns = new Dictionary<EnumsCollection.SkillSequencePatternID, List<bool>>();
            for (int i = 0; i < SkillSequencePatterns.Count; i++)
            {
                if (!m_SkillSequencePatterns.ContainsKey(SkillSequencePatterns[i].ID))
                    m_SkillSequencePatterns.Add(SkillSequencePatterns[i].ID, SkillSequencePatterns[i].Pattern);
            }
        }

        [System.Serializable]
        public class SequencePatternData
        {
            public EnumsCollection.SkillSequencePatternID ID;
            public List<bool> Pattern;
        }
    }
}
