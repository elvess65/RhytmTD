using RhytmTD.Assets.Abstract;
using UnityEngine;

namespace RhytmTD.Assets.Battle
{
    [CreateAssetMenu(fileName = "New Level PrefabsLibrary", menuName = "Assets/Level Prefabs Library", order = 101)]
    public class LevelPrefabAssets : PrefabAssets
    {
        public GameObject EnemyPrefab;

        public override void Initialize()
        {
        }
    }
}
