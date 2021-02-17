using RhytmTD.Assets.Abstract;
using RhytmTD.Battle.Entities.Views;
using UnityEngine;

namespace RhytmTD.Assets.Battle
{
    [CreateAssetMenu(fileName = "New Battle PrefabsLibrary", menuName = "Assets/Battle Prefabs Library", order = 101)]
    public class BattlePrefabAssets : PrefabAssets
    {
        public PlayerView PlayerPrefab;
        public ProjectileView SimpleAttackProjectilePrefab;

        public override void Initialize()
        {
        }
    }
}
