using CoreFramework;
using RhytmTD.Assets.Battle;
using RhytmTD.Battle.Entities.Controllers;
using RhytmTD.Battle.Entities.Models;
using System;
using UnityEngine;

namespace RhytmTD.Battle.Entities.Views
{
    /// <summary>
    /// Отображение содержащие точки спауна
    /// </summary>
    public class SpawnView : BaseView, IDisposable
    {
        [SerializeField] private Transform PlayerSpawnArea;
        [SerializeField] private Transform[] EnemySpawnAreas;

        private SpawnModel m_SpawnModel;

        void Awake()
        {
            Initialize();
        }

        private void Initialize()
        {
            Dispatcher.AddDisposable(this);

            m_SpawnModel = Dispatcher.GetModel<SpawnModel>();
            m_SpawnModel.PlayerSpawnPosition = PlayerSpawnArea.position;
            m_SpawnModel.EnemySpawnPosition = new Vector3[EnemySpawnAreas.Length];
            m_SpawnModel.OnPlayerCreated += SpawnPlayer;
            m_SpawnModel.OnEnemyCreated += SpawnEnemy;
            m_SpawnModel.OnBulletCreated += SpawnBullet;

            for (int i = 0; i < EnemySpawnAreas.Length; ++i)
            {
                m_SpawnModel.EnemySpawnPosition[i] = EnemySpawnAreas[i].transform.position;
            }

            m_SpawnModel.SpawnsInitialized?.Invoke();
        }

        private void SpawnPlayer(int typeID, BattleEntity battleEntity)
        {
            TransformModule transformModule = battleEntity.GetModule<TransformModule>();

            //Create View
            GameObject player = BattleAssetsManager.Instance.GetAssets().InstantiateGameObject(BattleAssetsManager.Instance.GetAssets().PlayerPrefab);
            player.transform.position = transformModule.Position;
            player.transform.rotation = transformModule.Rotation;

            BattleEntityView playerView = player.GetComponent<BattleEntityView>();

            //Initialize Entity
            playerView.Initialize(battleEntity);
        }

        private void SpawnEnemy(int typeID, BattleEntity battleEntity)
        {
            TransformModule transformModule = battleEntity.GetModule<TransformModule>();

            //Create View
            GameObject enemy = BattleAssetsManager.Instance.GetAssets().InstantiateGameObject(BattleAssetsManager.Instance.GetAssets().EnemyPrefab);
            enemy.transform.position = transformModule.Position;
            enemy.transform.rotation = transformModule.Rotation;

            BattleEntityView enemyView = enemy.GetComponent<BattleEntityView>();

            //Initialize Entity
            enemyView.Initialize(battleEntity);
        }

        private void SpawnBullet(int typeID, BattleEntity battleEntity)
        {
            TransformModule transformModule = battleEntity.GetModule<TransformModule>();

            GameObject bulletObj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            bulletObj.transform.localScale = Vector3.one * 0.2f;
            bulletObj.transform.position = transformModule.Position;

            bulletObj.GetComponent<Renderer>().material.color = Color.black;

            BulletView bulletView = bulletObj.AddComponent<BulletView>();
            bulletView.Initialize(battleEntity);
        }


        public void Dispose()
        {
            m_SpawnModel.OnPlayerCreated -= SpawnPlayer;
            m_SpawnModel.OnEnemyCreated -= SpawnEnemy;
            m_SpawnModel.OnBulletCreated -= SpawnBullet;

            Dispatcher.RemoveDisposable(this);
        }

        private void OnDrawGizmos()
        {
            Color initColor = Gizmos.color;
            Gizmos.color = Color.yellow;

            for (int i = 0; i < EnemySpawnAreas.Length; i++)
            {
                if (m_SpawnModel == null)
                    Gizmos.DrawWireSphere(EnemySpawnAreas[i].position, 1);
                else
                    Gizmos.DrawWireSphere(m_SpawnModel.EnemySpawnPosition[i], 1);
            }

            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(PlayerSpawnArea.position, 1);
            Gizmos.color = initColor;
        }
    }
}
