using UnityEngine;

namespace RhytmTD.Data.DataBaseLocal
{
    /// <summary>
    /// Local data base 
    /// </summary>
    [System.Serializable]
    public class DBLocal : MonoBehaviour
    {
        public DBLocal_AccountData AccountData;
        public DBLocal_AccountLevelingData AccountLevelingData;
        public DBLocal_AccountBaseParamsData AccountBaseParamsData;
        public DBLocal_SkillSequenceData SkillSequenceData;
        public DBLocal_EnvironmentData EnvironmentData;
        public DBLocal_WorldData WorldData;
    }
}
