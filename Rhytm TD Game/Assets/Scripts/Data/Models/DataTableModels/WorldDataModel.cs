using System.Collections.Generic;

namespace RhytmTD.Data.Models.DataTableModels
{
    /// <summary>
    /// Информация о мире, который содержит зоны, которые рассчитывают данные для уровней и волн
    /// </summary>
    public class WorldDataModel : DeserializableDataModel<WorldDataModel>
    {
        /// <summary>
        /// Используеться только для парсинга данных
        /// </summary>
        public AreaData[] Areas;

        private Dictionary<int, AreaData> m_Areas;


        public AreaData GetAreaDataByID(int areaID)
        {
            if (m_Areas.ContainsKey(areaID))
                return m_Areas[areaID];

            return null;
        }

        public override void ReorganizeData()
        {
            m_Areas = new Dictionary<int, AreaData>();
            for (int i = 0; i < Areas.Length; i++)
            {
                if (!m_Areas.ContainsKey(Areas[i].ID))
                {
                    m_Areas.Add(Areas[i].ID, Areas[i]);
                }
            }
        }


        /// <summary>
        /// Данные о зоне, которая содержит данные для построения уровней
        /// Зона принадлежит модели, так как содержит данные из базы данных,
        /// а уровни принадлежат спауну, так как не содержат данных из базы
        /// </summary>
        [System.Serializable]
        public class AreaData
        {
            public ProgressionConfig Enemies;
            public ProgressionConfig AttackTicks;
            public ProgressionConfig RestTicks;

            public int ID;
            public int WavesAmount;
            public int TotalLevels;

            public int CompletedLevels = 2;

            public float CompletionProgress01 => CompletedLevels / (float)TotalLevels;
        }
    }
}
