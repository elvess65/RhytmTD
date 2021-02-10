using RhytmTD.Battle.Core;
using RhytmTD.Battle.Entities;
using RhytmTD.Battle.Entities.EntitiesFactory;
using RhytmTD.Battle.Entities.Views;
using UnityEngine;

namespace RhytmTD.Battle.Spawn
{
    public class EntitySpawner : MonoBehaviour
    {
        [SerializeField] private Transform PlayerSpawnArea;
        [SerializeField] private Transform[] EnemySpawnAreas;

        private IBattleEntityFactory m_EnemiesFactory;
        private IBattleEntityFactory m_PlayerFactory;
        private Transform m_PlayerTransform;
        private int[] m_SpawnAreaUsedAmount;
        private Vector3[] m_EnemySpawnAreasOffsets;

        private Vector3 m_AREA_USED_OFFSET = new Vector3(0, 0, 2);

        void Awake()
        {
            m_SpawnAreaUsedAmount = new int[EnemySpawnAreas.Length];
            m_EnemySpawnAreasOffsets = new Vector3[EnemySpawnAreas.Length];
            m_PlayerFactory = new DefaultPlayerFactory();
            m_EnemiesFactory = new DefaultEnemyFactory();
        }


        public BattleEntity SpawnPlayer(EntityFactorySetup setup)
        {
            //Create View
            GameObject player = BattleManager.Instance.MonoReferencesHolder.AssetsManager.GetAssets().InstantiateGameObject(BattleManager.Instance.MonoReferencesHolder.AssetsManager.GetAssets().PlayerPrefab);
            player.transform.position = PlayerSpawnArea.position;

            //Create Entity
            BattleEntity battleEntity = m_PlayerFactory.CreateEntity(player.transform, setup);
            BattleEntityView playerView = player.GetComponent<BattleEntityView>();

            //Initialize Entity
            playerView.Initialize(battleEntity);

            return battleEntity;
        }

        public BattleEntity SpawnEnemy(EntityFactorySetup setup)
        {
            //Span Area indexes
            int randomSpawnAreaIndex = Random.Range(0, EnemySpawnAreas.Length);
            int spawnAreaUsedAmount = m_SpawnAreaUsedAmount[randomSpawnAreaIndex]++;

            //Create View
            GameObject enemy = BattleManager.Instance.MonoReferencesHolder.AssetsManager.GetAssets().InstantiateGameObject(BattleManager.Instance.MonoReferencesHolder.AssetsManager.GetAssets().EnemyPrefab);
            enemy.transform.position = EnemySpawnAreas[randomSpawnAreaIndex].position + m_AREA_USED_OFFSET * spawnAreaUsedAmount;
            enemy.transform.rotation = Quaternion.Euler(enemy.transform.rotation.eulerAngles.x, Random.rotation.eulerAngles.y, enemy.transform.rotation.eulerAngles.z);

            //Create Entity
            BattleEntity battleEntity = m_EnemiesFactory.CreateEntity(enemy.transform, setup);
            BattleEntityView enemyView = enemy.GetComponent<BattleEntityView>();

            //Initialize Entity
            enemyView.Initialize(battleEntity);

            return battleEntity;
        }

        public void ResetSpawnAreaUsedAmount()
        {
            for (int i = 0; i < m_SpawnAreaUsedAmount.Length; i++)
            {
                m_SpawnAreaUsedAmount[i] = 0;
            }
        }

        public void CacheSpawnAreaPosition(Transform playerTransform)
        {
            m_PlayerTransform = playerTransform;

            for (int i = 0; i < EnemySpawnAreas.Length; i++)
            {
                m_EnemySpawnAreasOffsets[i] = EnemySpawnAreas[i].position - m_PlayerTransform.position;
            }
        }

        public void AdjustSpawnAreaPosition()
        {
            for(int i = 0; i < EnemySpawnAreas.Length; i++)
            {
                EnemySpawnAreas[i].transform.position = m_PlayerTransform.position + m_EnemySpawnAreasOffsets[i];
            }
        }


        private void OnDrawGizmos()
        {
            Color initColor = Gizmos.color;
            Gizmos.color = Color.red;
            for (int i = 0; i < EnemySpawnAreas.Length; i++)
                Gizmos.DrawWireSphere(EnemySpawnAreas[i].position, 1);

            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(PlayerSpawnArea.position, 1);
            Gizmos.color = initColor;
        }
    }
}
