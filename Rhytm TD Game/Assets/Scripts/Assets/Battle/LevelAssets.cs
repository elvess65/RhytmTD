using RhytmTD.Assets.Abstract;
using RhytmTD.Battle.Entities.Views;
using UnityEngine;

namespace RhytmTD.Assets.Battle
{
    [CreateAssetMenu(fileName = "New Level Assets", menuName = "Assets/Level Assets", order = 101)]
    public class LevelAssets : PrefabAssets
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
