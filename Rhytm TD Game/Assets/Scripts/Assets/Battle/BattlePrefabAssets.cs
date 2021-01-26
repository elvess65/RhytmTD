using RhytmTD.Assets.Abstract;
using UnityEngine;

namespace RhytmTD.Assets.Battle
{
    [CreateAssetMenu(fileName = "New Battle PrefabsLibrary", menuName = "Assets/Battle Prefabs Library", order = 101)]
    public class BattlePrefabAssets : PrefabAssets
    {
        public GameObject ExamplePrefab;

        public override void Initialize()
        {
        }
    }
}
