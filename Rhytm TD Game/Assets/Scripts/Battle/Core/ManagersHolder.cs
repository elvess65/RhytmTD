using RhytmTD.Assets.Battle;
using RhytmTD.UI.Battle;
using UnityEngine;

namespace RhytmTD.Battle.Core
{
    /// <summary>
    /// Holder for managers
    /// </summary>
    public class ManagersHolder : MonoBehaviour
    {
        public BattleUIManager UIManager;
        public BattleAssetsManager AssetsManager;

        public void Initialize()
        {
            UIManager.Initialize();
            AssetsManager.Initialize();
        }
    }
}
