using System.Collections.Generic;
using UnityEngine;

namespace RhytmTD.Data.DataBaseLocal
{
    [System.Serializable]
    [CreateAssetMenu(fileName = "New LevelFactory", menuName = "DBSimulation/Levels/LevelFactory", order = 101)]
    public class LevelFactory : ScriptableObject
    {
        [System.NonSerialized]
        public int TestData1 = 100;
        public int TestData2 = 200;
        public int TestData3 = 300;
        public AnimationCurve Curve;
        public List<Wave> Waves = new List<Wave>();

        [System.Serializable]
        public class Wave
        {
            public int EnemiesAmount = 10;
            public int MinDamage = 20;
            public int MaxDamage = 30;
            public int MinHP = 20;
            public int MaxHP = 30;

            public List<Chunk> Chunks = new List<Chunk>();
        }

        [System.Serializable]
        public class Chunk
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
