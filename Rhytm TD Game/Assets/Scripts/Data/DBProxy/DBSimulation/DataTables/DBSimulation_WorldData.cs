using UnityEngine;

namespace RhytmTD.Data.DataBase.Simulation
{
    /// <summary>
    /// Мир, который содержит зоны
    /// Каждая зона строит уровни содержащимся в ней данным
    /// Уровни строят волны
    /// </summary>
    [System.Serializable]
    [CreateAssetMenu(fileName = "New Simulation WorldData", menuName = "DBSimulation/Levels/WorldData", order = 101)]
    public class DBSimulation_WorldData : ScriptableObject
    {
        public AreaData[] Areas;

        [System.Serializable]
        public class AreaData
        {
            public ProgressionConfig ProgressionEnemies;
            public ProgressionConfig ProgressionChunksAmount;
            public ProgressionConfig ProgressionRestTicks;
            public ProgressionConfig ProgressionDelayBetweenChunks;

            public int ID;
            public int WavesAmount;
            public int TotalLevels;
            public int DelayBeforeStartLevel;
        }
    }
}
