using RhytmTD.Assets.Abstract;
using RhytmTD.Battle.Entities.Views;
using UnityEngine;

namespace RhytmTD.Assets.Battle
{
    [CreateAssetMenu(fileName = "New Level PrefabsLibrary", menuName = "Assets/Level Prefabs Library", order = 101)]
    public class LevelPrefabAssets : PrefabAssets
    {
        [SerializeField] private EnemyView[] m_EnemyPrefabs;

        public override void Initialize()
        {
        }

        public EnemyView GetRandomEnemyViewPrefab()
        {
            int rndIndex = Random.Range(0, m_EnemyPrefabs.Length);
            return m_EnemyPrefabs[rndIndex];
        }
    }
}
