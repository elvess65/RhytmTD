using RhytmTD.Data.Factory;

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
            public int ID;
            public LevelDataFactory[] LevelsData;
        }
    }
}
