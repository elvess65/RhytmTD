using System.Collections.Generic;
using RhytmTD.Battle.Entities.EntitiesFactory;
using UnityEngine;

namespace RhytmTD.Data.Models.DataTableModels
{
    /// <summary>
    /// Информация о мире, который содержит зоны, которые рассчитывают данные для уровней и волн
    /// </summary>
    public class WorldDataModel : BaseModel
    {
        public AreaData[] Areas;

        /// <summary>
        /// Данные о зоне, которая содержит данные для построения уровней
        /// Зона принадлежит модели, так как содержит данные из базы данных,
        /// а уровни принадлежат спауну, так как не содержат данных из базы
        /// </summary>
        [System.Serializable]
        public class AreaData
        {
            public ProgressionConfig ProgressionEnemies;
            public ProgressionConfig ProgressionChunksAmount;
            public ProgressionConfig ProgressionRestTicks;
            public ProgressionConfig ProgressionDelayBetweenChunks;
            public BaseBattleEntityFactory EnemiesFactory;
            public LevelFactory LF;

            public int ID;
            public int WavesAmount;
            public int TotalLevels;
            public int DelayBeforeStartLevel;
        }

        [System.Serializable]
        public class LevelFactory
        {
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
}
