using CoreFramework;
using RhytmTD.Assets.Battle;
using RhytmTD.Battle.Entities.Controllers;
using RhytmTD.Battle.Entities.Models;
using RhytmTD.Data.Models;
using RhytmTD.Data.Models.DataTableModels;
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
        private AccountDataModel m_AccountModel;
        private WorldDataModel m_WorldModel;

        private void Awake()
        {
            Initialize();
        }

        private void Initialize()
        {
            Dispatcher.AddDisposable(this);

            m_AccountModel = Dispatcher.GetModel<AccountDataModel>();
            m_WorldModel = Dispatcher.GetModel<WorldDataModel>();

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

            m_SpawnModel.OnSpawnPointsInitialized?.Invoke();
        }

        private void SpawnPlayer(int typeID, BattleEntity battleEntity)
        {
            TransformModule transformModule = battleEntity.GetModule<TransformModule>();

            //Create View
            BattlePrefabAssets assets = m_WorldModel.Assets;
            BattleEntityView playerView = assets.InstantiatePrefab(assets.PlayerPrefab);
            playerView.transform.position = transformModule.Position;
            playerView.transform.rotation = transformModule.Rotation;

            //Initialize Entity
            playerView.Initialize(battleEntity);
        }

        private void SpawnEnemy(int typeID, BattleEntity battleEntity)
        {
            TransformModule transformModule = battleEntity.GetModule<TransformModule>();

            //Create View
            LevelPrefabAssets assets = m_WorldModel.Areas[m_AccountModel.CompletedAreas].LevelsData[m_AccountModel.CompletedLevels].Assets;
            BattleEntityView enemyView = assets.InstantiatePrefab(assets.GetRandomEnemyViewPrefab());
            enemyView.transform.position = transformModule.Position;
            enemyView.transform.rotation = transformModule.Rotation;

            //Initialize Entity
            enemyView.Initialize(battleEntity);
        }

        private void SpawnBullet(int typeID, BattleEntity battleEntity)
        {
            TransformModule transformModule = battleEntity.GetModule<TransformModule>();

            //Create View
            BattlePrefabAssets assets = m_WorldModel.Assets;
            ProjectileView projectileView = assets.InstantiatePrefab(assets.SimpleAttackProjectilePrefab);
            projectileView.transform.localScale = Vector3.one;
            projectileView.transform.position = transformModule.Position;

            //Initialize Entity
            projectileView.Initialize(battleEntity);
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
