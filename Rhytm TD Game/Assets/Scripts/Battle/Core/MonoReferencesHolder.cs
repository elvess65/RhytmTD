using RhytmTD.Assets.Battle;
using RhytmTD.UI.Battle;
using UnityEngine;

namespace RhytmTD.Battle.Core
{
    /// <summary>
    /// Holder for required MonoBehaviours
    /// </summary>
    public class MonoReferencesHolder : MonoBehaviour
    {
        public BattleUIManager UIManager;
        public BattleAssetsManager AssetsManager;
        public WorldSpawner EnemySpawner;

        public void Initialize()
        {
            UIManager.Initialize();
            AssetsManager.Initialize();
        }
    }
}
