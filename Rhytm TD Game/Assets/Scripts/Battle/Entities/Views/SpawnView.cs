using CoreFramework;
using RhytmTD.Assets.Battle;
using RhytmTD.Battle.Entities;
using RhytmTD.Battle.Entities.EntitiesFactory;
using RhytmTD.Battle.Entities.Models;
using RhytmTD.Battle.Entities.Views;
using UnityEngine;

namespace RhytmTD.Battle.Spawn
{
    public class SpawnView : BaseView
    {
        [SerializeField] private Transform PlayerSpawnArea;
        [SerializeField] private Transform[] EnemySpawnAreas;

        private IBattleEntityFactory m_EnemiesFactory;
        private IBattleEntityFactory m_PlayerFactory;
        private TransformModule m_PlayerTransform;
        private int[] m_SpawnAreaUsedAmount;
        private Vector3[] m_EnemySpawnAreasOffsets;
        private SpawnModel m_SpawnModel;

        private Vector3 m_AREA_USED_OFFSET = new Vector3(0, 0, 2);

        void Awake()
        {
            Initialize();
        }

        private void Initialize()
        {
            m_SpawnAreaUsedAmount = new int[EnemySpawnAreas.Length];
            m_EnemySpawnAreasOffsets = new Vector3[EnemySpawnAreas.Length];
            m_PlayerFactory = new DefaultPlayerFactory();
            m_EnemiesFactory = new DefaultEnemyFactory();

            m_SpawnModel = Dispatcher.GetModel<SpawnModel>();
            m_SpawnModel.OnSpawnPlayerEntity += SpawnPlayer;
            m_SpawnModel.OnSpawnEnemyEntity += SpawnEnemy;
            m_SpawnModel.OnResetSpawnAreaUsedAmount += ResetSpawnAreaUsedAmount;
            m_SpawnModel.OnCacheSpawnAreaPosition += CacheSpawnAreaPosition;
            m_SpawnModel.OnAdjustSpawnAreaPosition += AdjustSpawnAreaPosition;
        }


        private BattleEntity SpawnPlayer(EntityFactorySetup setup)
        {
            //Create View
            GameObject player = BattleAssetsManager.Instance.GetAssets().InstantiateGameObject(BattleAssetsManager.Instance.GetAssets().PlayerPrefab);
            player.transform.position = PlayerSpawnArea.position;

            //Create Entity
            BattleEntity battleEntity = m_PlayerFactory.CreateEntity(player.transform, setup);
            BattleEntityView playerView = player.GetComponent<BattleEntityView>();

            //Initialize Entity
            playerView.Initialize(battleEntity);

            return battleEntity;
        }

        private BattleEntity SpawnEnemy(EntityFactorySetup setup)
        {
            //Span Area indexes
            int randomSpawnAreaIndex = Random.Range(0, EnemySpawnAreas.Length);
            int spawnAreaUsedAmount = m_SpawnAreaUsedAmount[randomSpawnAreaIndex]++;

            //Create View
            GameObject enemy = BattleAssetsManager.Instance.GetAssets().InstantiateGameObject(BattleAssetsManager.Instance.GetAssets().EnemyPrefab);
            enemy.transform.position = EnemySpawnAreas[randomSpawnAreaIndex].position + m_AREA_USED_OFFSET * spawnAreaUsedAmount;
            enemy.transform.rotation = Quaternion.Euler(enemy.transform.rotation.eulerAngles.x, Random.rotation.eulerAngles.y, enemy.transform.rotation.eulerAngles.z);

            //Create Entity
            BattleEntity battleEntity = m_EnemiesFactory.CreateEntity(enemy.transform, setup);
            BattleEntityView enemyView = enemy.GetComponent<BattleEntityView>();

            //Initialize Entity
            enemyView.Initialize(battleEntity);

            return battleEntity;
        }

        private void ResetSpawnAreaUsedAmount()
        {
            for (int i = 0; i < m_SpawnAreaUsedAmount.Length; i++)
            {
                m_SpawnAreaUsedAmount[i] = 0;
            }
        }

        public void CacheSpawnAreaPosition(TransformModule transformModule)
        {
            m_PlayerTransform = transformModule;

            for (int i = 0; i < EnemySpawnAreas.Length; i++)
            {
                m_EnemySpawnAreasOffsets[i] = EnemySpawnAreas[i].position - m_PlayerTransform.Position;
            }
        }

        private void AdjustSpawnAreaPosition()
        {
            for(int i = 0; i < EnemySpawnAreas.Length; i++)
            {
                EnemySpawnAreas[i].transform.position = m_PlayerTransform.Position + m_EnemySpawnAreasOffsets[i];
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
