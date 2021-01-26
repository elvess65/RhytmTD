using RhytmTD.Battle.Core;
using UnityEngine;

public class WorldSpawner : MonoBehaviour
{
    [SerializeField] private Transform[] SpawnAreas;

    private Vector3 m_AreaUsedOffset = new Vector3(0, 0, 2);
    private int[] m_SpanwAreaUsedAmount;

    public void Spawn(int amount)
    {
        int spawnedEnemies = 0;
        m_SpanwAreaUsedAmount = new int[SpawnAreas.Length];

        while (spawnedEnemies++ < amount)
        {
            int randomSpawnAreaIndex = Random.Range(0, SpawnAreas.Length);
            int spawnAreaUsedAmount = m_SpanwAreaUsedAmount[randomSpawnAreaIndex]++;

            SpawnEnemy(randomSpawnAreaIndex, spawnAreaUsedAmount);
        }
    }

    private void SpawnEnemy(int spawnAreaIndex, int usedAmount)
    {
        GameObject enemy = BattleManager.Instance.MonoReferencesHolder.AssetsManager.GetAssets().InstantiateGameObject(BattleManager.Instance.MonoReferencesHolder.AssetsManager.GetAssets().ExamplePrefab);
        enemy.transform.position = SpawnAreas[spawnAreaIndex].position + m_AreaUsedOffset * usedAmount;
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
