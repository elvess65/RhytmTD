using UnityEngine;

namespace RhytmTD.Data.DataBase.Simulation
{
    /// <summary>
    /// Мир, который содержит зоны
    /// </summary>
    [System.Serializable]
    [CreateAssetMenu(fileName = "New Simulation WorldData", menuName = "DBSimulation/Levels/WorldData", order = 101)]
    public class DBSimulation_WorldData : ScriptableObject
    {
        public AreaData[] Areas;

        [System.Serializable]
        public class AreaData
        {
            public ProgressionConfig Enemies;
            public ProgressionConfig AttackTicks;
            public ProgressionConfig RestTicks;

            public int ID;
            public int WavesAmount = 5;
            public int TotalLevels = 10;
        }
    }
}
