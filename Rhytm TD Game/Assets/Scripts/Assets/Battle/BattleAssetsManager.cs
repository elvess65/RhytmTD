using RhytmTD.Assets.Abstract;

namespace RhytmTD.Assets.Battle
{
    /// <summary>
    /// Обвертка вокруг Generic Assets Manager (так как MonoBehaviour не умеет работать с Generic)
    /// </summary>
    public class BattleAssetsManager : AssetsManager<BattlePrefabAssets>
    {
    }
}
