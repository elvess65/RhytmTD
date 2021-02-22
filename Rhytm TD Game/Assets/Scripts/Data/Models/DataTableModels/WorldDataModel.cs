using CoreFramework;
using RhytmTD.Assets.Battle;
using RhytmTD.Data.Factory;

namespace RhytmTD.Data.Models.DataTableModels
{
    /// <summary>
    /// Информация о мире, который содержит зоны, которые рассчитывают данные для уровней и волн
    /// </summary>
    public class WorldDataModel : BaseModel
    {
        public PlayerCharacterAssets PlayerCharacterAssets;
        public EffectAssets EffectAssets;
        public AreaData[] Areas;

        public override void Initialize()
        {
            base.Initialize();

            PlayerCharacterAssets.Initialize();
            EffectAssets.Initialize();
        }

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
