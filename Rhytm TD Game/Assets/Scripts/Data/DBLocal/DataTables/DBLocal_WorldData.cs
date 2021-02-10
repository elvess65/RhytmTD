using RhytmTD.Battle.Entities.EntitiesFactory;
using UnityEngine;

namespace RhytmTD.Data.DataBaseLocal
{
    /// <summary>
    /// Мир, который содержит зоны
    /// Каждая зона строит уровни содержащимся в ней данным
    /// Уровни строят волны
    /// </summary>
    [System.Serializable]
    [CreateAssetMenu(fileName = "New Local WorldData", menuName = "DBLocal/Levels/WorldData", order = 101)]
    public class DBLocal_WorldData : ScriptableObject
    {
        public AreaData[] Areas;

        [System.Serializable]
        public class AreaData
        {
            public int ID;
            public int TotalLevels;

            public ProgressionConfig ProgressionEnemies;
            public ProgressionConfig ProgressionChunksAmount;
            public ProgressionConfig ProgressionRestTicks;
            public ProgressionConfig ProgressionDelayBetweenChunks;
            public BaseBattleEntityFactory EnemiesFactory;
            public LevelFactory LF;

            public int WavesAmount;
            public int DelayBeforeStartLevel;
        }
    }
}
