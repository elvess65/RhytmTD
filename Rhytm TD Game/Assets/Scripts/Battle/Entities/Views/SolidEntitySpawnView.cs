using CoreFramework;
using RhytmTD.Assets.Battle;
using RhytmTD.Battle.Entities.Models;
using RhytmTD.Data.Models;
using RhytmTD.Data.Models.DataTableModels;
using System;
using UnityEngine;

namespace RhytmTD.Battle.Entities.Views
{
    /// <summary>
    /// Links world spawn points with entities 
    /// Creates views for solid entities
    /// </summary>
    public class SolidEntitySpawnView : BaseView, IDisposable
    {
        [SerializeField] private Transform PlayerSpawnArea = null;
        [SerializeField] private Transform[] EnemySpawnAreas = null;

        private SpawnModel m_SpawnModel;
        private WorldDataModel m_WorldModel;
        private AccountDataModel m_AccountModel;

        private void Awake()
        {
            Initialize();
        }

        private void Initialize()
        {
            Dispatcher.AddDisposable(this);

            m_WorldModel = Dispatcher.GetModel<WorldDataModel>();
            m_AccountModel = Dispatcher.GetModel<AccountDataModel>();

            m_SpawnModel = Dispatcher.GetModel<SpawnModel>();
            m_SpawnModel.PlayerSpawnPosition = PlayerSpawnArea.position;
            m_SpawnModel.EnemySpawnPosition = new Vector3[EnemySpawnAreas.Length];

            m_SpawnModel.OnPlayerEntityCreated += CreatePlayerView;
            m_SpawnModel.OnEnemyEntityCreated += CreateEnemyView;

            for (int i = 0; i < EnemySpawnAreas.Length; ++i)
            {
                m_SpawnModel.EnemySpawnPosition[i] = EnemySpawnAreas[i].transform.position;
            }

            m_SpawnModel.OnSpawnPointsInitialized?.Invoke();
        }

        private void CreatePlayerView(int typeID, BattleEntity battleEntity)
        {
            TransformModule transformModule = battleEntity.GetModule<TransformModule>();

            //Create View
            BattleEntityView playerView = m_WorldModel.PlayerCharacterAssets.InstantiatePrefab(m_WorldModel.PlayerCharacterAssets.PlayerPrefab);
            playerView.transform.position = transformModule.Position;
            playerView.transform.rotation = transformModule.Rotation;

            //Initialize Entity
            playerView.Initialize(battleEntity);
        }

        private void CreateEnemyView(int typeID, BattleEntity battleEntity)
        {
            TransformModule transformModule = battleEntity.GetModule<TransformModule>();

            //Create View
            LevelAssets assets = m_WorldModel.Areas[m_AccountModel.CompletedAreas].LevelsData[m_AccountModel.CompletedLevels].Assets;
            BattleEntityView enemyView = assets.InstantiatePrefab(assets.GetRandomEnemyViewPrefab());
            enemyView.transform.position = transformModule.Position;
            enemyView.transform.rotation = transformModule.Rotation;

            //Initialize Entity
            enemyView.Initialize(battleEntity);
        }


        public void Dispose()
        {
            m_SpawnModel.OnPlayerEntityCreated -= CreatePlayerView;
            m_SpawnModel.OnEnemyEntityCreated -= CreateEnemyView;

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
