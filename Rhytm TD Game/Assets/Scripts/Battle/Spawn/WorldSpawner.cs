using RhytmTD.Battle.Core;
using System.Collections.Generic;
using UnityEngine;

namespace RhytmTD.Battle.Spawn
{
    public class WorldSpawner : MonoBehaviour
    {
        [SerializeField] private Transform[] SpawnAreas;

        private Vector3 m_AreaUsedOffset = new Vector3(0, 0, 2);
        private int[] m_SpanwAreaUsedAmount;
        private List<GameObject> m_SpawnedEntitiesContainer = new List<GameObject>();

        public List<GameObject> Spawn(int amount)
        {
            m_SpawnedEntitiesContainer.Clear();

            int spawnedEnemies = 0;
            m_SpanwAreaUsedAmount = new int[SpawnAreas.Length];

            while (spawnedEnemies++ < amount)
            {
                int randomSpawnAreaIndex = Random.Range(0, SpawnAreas.Length);
                int spawnAreaUsedAmount = m_SpanwAreaUsedAmount[randomSpawnAreaIndex]++;

                m_SpawnedEntitiesContainer.Add(SpawnEnemy(randomSpawnAreaIndex, spawnAreaUsedAmount));
            }

            return m_SpawnedEntitiesContainer;
        }

        private GameObject SpawnEnemy(int spawnAreaIndex, int usedAmount)
        {
            GameObject enemy = BattleManager.Instance.MonoReferencesHolder.AssetsManager.GetAssets().InstantiateGameObject(BattleManager.Instance.MonoReferencesHolder.AssetsManager.GetAssets().ExamplePrefab);
            enemy.transform.position = SpawnAreas[spawnAreaIndex].position + m_AreaUsedOffset * usedAmount;

            return enemy;
        }

        private void OnDrawGizmos()
        {
            if (SpawnAreas == null)
                return;

            for (int i = 0; i < SpawnAreas.Length; i++)
            {
                Gizmos.DrawWireSphere(SpawnAreas[i].position, 1);
            }
        }
    }
}
