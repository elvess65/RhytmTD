﻿namespace RhytmTD.Data.Models.DataTableModels
{
    /// <summary>
    /// Информация о мире, который содержит зоны, которые рассчитывают данные для уровней и волн
    /// </summary>
    public class WorldDataModel : DeserializableDataModel<WorldDataModel>
    {
        public AreaData[] Areas;

        public override void ReorganizeData()
        {
        }


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

            public int ID;
            public int WavesAmount;
            public int TotalLevels;
            public int DelayBeforeStartLevel;
        }
    }
}
