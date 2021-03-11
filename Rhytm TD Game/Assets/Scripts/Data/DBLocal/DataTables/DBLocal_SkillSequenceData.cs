using System.Collections.Generic;
using UnityEngine;
using static CoreFramework.EnumsCollection;

namespace RhytmTD.Data.DataBaseLocal
{
    [System.Serializable]
    [CreateAssetMenu(fileName = "New Local SkillSequenceData", menuName = "DBLocal/SkillSequenceData", order = 101)]
    public class DBLocal_SkillSequenceData : ScriptableObject
    {
        public List<SequencePatternData> SkillSequencePatterns;

        [System.Serializable]
        public class SequencePatternData
        {
            public SkillSequencePatternID ID;
            public List<bool> Pattern;

            public SequencePatternData()
            {
                ID = SkillSequencePatternID.Pattern1;

                Pattern = new List<bool>();
                for (int i = 0; i < 5; i++)
                    Pattern.Add(false);
            }
        }
    }
}
