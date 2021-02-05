﻿using RhytmTD.Battle.Core;
using RhytmTD.Battle.Entities;
using RhytmTD.Battle.Entities.EntitiesFactory;
using RhytmTD.Battle.Entities.Views;
using UnityEngine;

namespace RhytmTD.Battle.Spawn
{
    public class WorldSpawner : MonoBehaviour
    {
        [SerializeField] private Transform PlayerSpawnArea;
        [SerializeField] private Transform[] EnemySpawnAreas;
        [SerializeField] private BaseBattleEntityFactory PlayerFactory;
        [SerializeField] private BaseBattleEntityFactory EnemyFactory;

        private Vector3 m_AREA_USED_OFFSET = new Vector3(0, 0, 2);
        private int[] m_SpanwAreaUsedAmount;

        void Awake()
        {
            m_SpanwAreaUsedAmount = new int[EnemySpawnAreas.Length];
        }

        public BattleEntity SpawnPlayer()
        {
            GameObject player = BattleManager.Instance.MonoReferencesHolder.AssetsManager.GetAssets().InstantiateGameObject(BattleManager.Instance.MonoReferencesHolder.AssetsManager.GetAssets().PlayerPrefab);
            player.transform.position = PlayerSpawnArea.position;

            BattleEntity battleEntity = PlayerFactory.CreateEntity(player.transform);
            BattleEntityView playerView = player.GetComponent<BattleEntityView>();

            playerView.Initialize(battleEntity);

            return battleEntity;
        }

        public BattleEntity SpawnEnemy()
        {
            int randomSpawnAreaIndex = Random.Range(0, EnemySpawnAreas.Length);
            int spawnAreaUsedAmount = m_SpanwAreaUsedAmount[randomSpawnAreaIndex]++;

            GameObject enemy = BattleManager.Instance.MonoReferencesHolder.AssetsManager.GetAssets().InstantiateGameObject(BattleManager.Instance.MonoReferencesHolder.AssetsManager.GetAssets().EnemyPrefab);
            enemy.transform.position = EnemySpawnAreas[randomSpawnAreaIndex].position + m_AREA_USED_OFFSET * spawnAreaUsedAmount;
            enemy.transform.rotation = Quaternion.Euler(enemy.transform.rotation.eulerAngles.x, Random.rotation.eulerAngles.y, enemy.transform.rotation.eulerAngles.z); 

            BattleEntity battleEntity = EnemyFactory.CreateEntity(enemy.transform);
            BattleEntityView enemyView = enemy.GetComponent<BattleEntityView>();

            enemyView.Initialize(battleEntity);

            return battleEntity;
        }

        public void ResetSpawnAreas()
        {
            for (int i = 0; i < m_SpanwAreaUsedAmount.Length; i++)
            {
                m_SpanwAreaUsedAmount[i] = 0;
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
