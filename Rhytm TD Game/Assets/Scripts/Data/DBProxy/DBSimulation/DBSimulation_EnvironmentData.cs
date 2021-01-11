using UnityEngine;

namespace RhytmTD.Data.DataBase.Simulation
{
    [System.Serializable]
    [CreateAssetMenu(fileName = "New Simulation EnvironmentData", menuName = "DBSimulation/EnvironmentData", order = 101)]
    public class DBSimulation_EnvironmentData : ScriptableObject
    {
        [Header("Данные для построения уровней")]

        public LevelParams[] LevelParamsData;

        [System.Serializable]
        public class LevelParams
        {
            [Header("General")]
            public int ID = 1;
            public int BPM = 130;

            [Header("BuildParams")]
            public BuildData BuildParams;

            [Header("ContentParams")]
            public ContentData ContentParams;
        }

        [System.Serializable]
        public class BuildData
        {
            [Header("Seed")]
            public bool OverrideSeed = true;
            public int LevelSeed = 10;
        }

        [System.Serializable]
        public class ContentData
        {
            [Header("Enemy Data")]
            public NPCProgressionConfig EnemyDataProgressionConfig;
            public NPCProgressionConfig BossDataProgressionConfig;
        }
    }
}
