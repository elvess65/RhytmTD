using RhytmTD.Assets.Battle;
using System.Collections.Generic;
using UnityEngine;

namespace RhytmTD.Data.Factory
{
    [System.Serializable]
    [CreateAssetMenu(fileName = "New LevelDataFactory", menuName = "DBLocal/Levels/LevelDataFactory", order = 101)]
    public class LevelDataFactory : ScriptableObject
    {
        public int DelayBeforeStartLevel = 2;
        public int RecomendedAverageDmg = 15;  
        public int DynamicDifficutlyReducePercent = 50;  

        public LevelAssets Assets;
        public List<WaveDataFactory> Waves = new List<WaveDataFactory>();

        [System.Serializable]
        public class WaveDataFactory
        {
            public int DurationRestTicks = 3;

            public int EnemiesAmount = 10;
            public int MinDamage = 20;
            public int MaxDamage = 30;
            public int MinHP = 20;
            public int MaxHP = 30;

            public List<ChunkDataFactory> Chunks = new List<ChunkDataFactory>();
        }

        [System.Serializable]
        public class ChunkDataFactory
        {
            public bool OverrideAmount = false;
            public bool OverrideHP = false;
            public bool OverrideDamage = false;

            public int EnemiesAmount = 10;
            public int MinDamage = 20;
            public int MaxDamage = 30;
            public int MinHP = 20;
            public int MaxHP = 30;
        }
    }
}
