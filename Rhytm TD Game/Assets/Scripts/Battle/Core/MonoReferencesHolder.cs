using RhytmTD.Assets.Battle;
using RhytmTD.Battle.Spawn;
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
        public EntitySpawner EntitySpawner;

        public void Initialize()
        {
            UIManager.Initialize();
            AssetsManager.Initialize();
        }
    }
}
