using RhytmTD.Assets.Abstract;

namespace RhytmTD.Assets.Battle
{
    /// <summary>
    /// Обвертка вокруг Generic Assets Manager (так как MonoBehaviour не умеет работать с Generic)
    /// </summary>
    public class LevelAssetsManager : AssetsManager<LevelPrefabAssets>
    {
        public static LevelAssetsManager Instance; //TEMP

        protected override void Initialize()
        {
            base.Initialize();

            Instance = this;
        }
    }
}
