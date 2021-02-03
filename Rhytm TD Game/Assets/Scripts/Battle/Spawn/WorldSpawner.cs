using RhytmTD.Battle.Core;
using RhytmTD.Battle.Entities.Views;
using System.Collections.Generic;
using UnityEngine;

namespace RhytmTD.Battle.Spawn
{
    public class WorldSpawner : MonoBehaviour
    {
        [SerializeField] private Transform PlayerSpawnArea;
        [SerializeField] private Transform[] EnemySpawnAreas;

        private Vector3 m_AREA_USED_OFFSET = new Vector3(0, 0, 2);
        private int[] m_SpanwAreaUsedAmount;
        private List<BaseBattleEntityView> m_SpawnedEntitiesContainer = new List<BaseBattleEntityView>();

        public BaseBattleEntityView[] SpawnEnemyViews(int amount)
        {
            m_SpawnedEntitiesContainer.Clear();

            int spawnedEnemies = 0;
            m_SpanwAreaUsedAmount = new int[EnemySpawnAreas.Length];

            while (spawnedEnemies++ < amount)
            {
                int randomSpawnAreaIndex = Random.Range(0, EnemySpawnAreas.Length);
                int spawnAreaUsedAmount = m_SpanwAreaUsedAmount[randomSpawnAreaIndex]++;

                Vector3 spawnPosition = EnemySpawnAreas[randomSpawnAreaIndex].position + m_AREA_USED_OFFSET * spawnAreaUsedAmount;
                m_SpawnedEntitiesContainer.Add(SpawnSingleView(spawnPosition));
            }

            return m_SpawnedEntitiesContainer.ToArray();
        }

        public BaseBattleEntityView SpawnPlayerView()
        {
            Vector3 spawnPosition = PlayerSpawnArea.position;
            return SpawnSingleView(spawnPosition);
        }

        private BaseBattleEntityView SpawnSingleView(Vector3 pos)
        {
            GameObject enemy = BattleManager.Instance.MonoReferencesHolder.AssetsManager.GetAssets().InstantiateGameObject(BattleManager.Instance.MonoReferencesHolder.AssetsManager.GetAssets().ExamplePrefab);
            enemy.transform.position = pos;

            BaseBattleEntityView entityView = enemy.AddComponent<BaseBattleEntityView>();

            return entityView;
        }

        private void OnDrawGizmos()
        {
            if (EnemySpawnAreas == null)
                return;

            Color initColor = Gizmos.color;
            Gizmos.color = Color.red;
            for (int i = 0; i < EnemySpawnAreas.Length; i++)
            {
                Gizmos.DrawWireSphere(EnemySpawnAreas[i].position, 1);
            }

            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(PlayerSpawnArea.position, 1);
            Gizmos.color = initColor;
        }
    }
}
