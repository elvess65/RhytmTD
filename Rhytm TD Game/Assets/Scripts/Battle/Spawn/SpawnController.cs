#define LOG_SPAWN
//#define SINGLE_SPAWN

using CoreFramework;
using RhytmTD.Battle.Entities;
using RhytmTD.Battle.Entities.Models;
using RhytmTD.Data.Factory;
using RhytmTD.Data.Models.DataTableModels;
using static RhytmTD.Data.Models.DataTableModels.WorldDataModel;

namespace RhytmTD.Battle.Spawn
{
    /// <summary>
    /// Контроллер создания врагов
    /// </summary>
    public class SpawnController : BaseController
    {
        public System.Action OnLevelComplete;
        public System.Action OnLevelFailed;

        private EntitySpawner m_EntitySpawner;
        private BattleModel m_BattleModel;
        private WorldDataModel m_WorldDataModel;
        private WorldDataModel.AreaData m_AreaData;

        private int m_ActionTargetTick;

        private int m_LevelIndex;
        private int m_WaveIndex;
        private int m_ChunkIndex;

        private LevelDataFactory m_CurrentLevel => m_AreaData.LevelsData[m_LevelIndex];
        private LevelDataFactory.WaveDataFactory m_CurrentWave => m_CurrentLevel.Waves[m_WaveIndex];
        private LevelDataFactory.ChunkDataFactory m_CurrentChunk => m_CurrentWave.Chunks[m_ChunkIndex];

        public SpawnController(Dispatcher dispatcher) : base(dispatcher)
        {  
        }

        public override void InitializeComplete()
        {
            m_BattleModel = Dispatcher.GetModel<BattleModel>();
            m_WorldDataModel = Dispatcher.GetModel<WorldDataModel>();
        }

        public void BuildLevel(EntitySpawner entityViewSpawner, WorldDataModel.AreaData areaData, int currentTick, float levelProgress01)
        {
            m_EntitySpawner = entityViewSpawner;
            //m_EntitySpawner.InjectEnemyFactory(areaData.EnemiesFactory);

            m_AreaData = areaData;

            SpawnPlayer();

            //Запланировать тик действия
            m_ActionTargetTick = currentTick + m_CurrentLevel.DelayBeforeStartLevel;
        }

        public void HandleTick(int ticksSinceStart)
        {
            if (m_ActionTargetTick == ticksSinceStart)
            {
                m_EntitySpawner.AdjustSpawnAreaPosition();
                //SpawnChunk(m_CurrentChunk.EnemiesAmount);
                Log($"Current tick: {ticksSinceStart}. Spawn wave index: {m_WaveIndex}. Enemies: {m_CurrentChunk.EnemiesAmount}", true);

                m_ChunkIndex++;

                //If all chunks from wave spawned
                if (m_ChunkIndex >= m_CurrentWave.Chunks.Count)
                {
                    Log($"All chunks spawned. Increment wave");

                    m_WaveIndex++;

                    if (m_WaveIndex >= m_CurrentLevel.Waves.Count)
                    {
                        //Reset processed waves amount
                        m_WaveIndex = 0;

                        //Reset processed chunks amount
                        m_ChunkIndex = 0;

                        //Get next level
                        m_LevelIndex++;

                        if (m_LevelIndex >= m_AreaData.LevelsData.Length)
                        {
                            //Stop scheduling tasks
                            m_ActionTargetTick = -1;

                            Log("Area is complete");
                        }
                        else
                        {
                            //Запланировать тик действия
                            m_ActionTargetTick += m_CurrentLevel.DelayBeforeStartLevel;

                            Log($"All waves spawned. Increment level {m_ActionTargetTick}");
                        }
                    }
                    else
                    {
                        //Schedule rest delay
                        m_ActionTargetTick += m_CurrentWave.DurationRestTicks;

                        //Reset processed chunks amount
                        m_ChunkIndex = 0;

                        Log($"Wave finished. Next wave at tick {m_ActionTargetTick}");
                    }
                }
                else
                {
                    //Schedule delay between chunks
                    m_ActionTargetTick += m_CurrentWave.DelayBetweenChunksTicks;

                    Log($"Chunk spawned. Next chunk spawn at {m_ActionTargetTick}. Left {m_CurrentWave.Chunks.Count - m_ChunkIndex}/{m_CurrentWave.Chunks.Count}");
                }

#if SINGLE_SPAWN
                m_ActionTargetTick = 0;
#endif
            }
        }


        private void SpawnChunk(int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                BattleEntity enemy = m_EntitySpawner.SpawnEnemy(0);
                m_BattleModel.AddBattleEntity(enemy);

                if (enemy.HasModule<HealthModule>())
                    enemy.GetModule<HealthModule>().OnDestroyed += EnemyEntity_OnDestroyed;
            }

            m_EntitySpawner.ResetSpawnAreas();
        }

        private void SpawnPlayer()
        {
            BattleEntity entity = m_EntitySpawner.SpawnPlayer();
            m_BattleModel.AddBattleEntity(entity);
            m_BattleModel.PlayerEntity = entity;

            m_EntitySpawner.CacheSpawnAreaPosition(entity.GetModule<TransformModule>().Transform);
            entity.GetModule<HealthModule>().OnDestroyed += PlayerEntity_OnDestroyed;
        }

        private void EnemyEntity_OnDestroyed(int entityID)
        {
            m_BattleModel.RemoveBattleEntity(entityID);

            //if (m_BattleModel.BattleEntities.Count == 1 && !m_Level.HasWaves)
            {
                OnLevelComplete?.Invoke();
            }
        }

        private void PlayerEntity_OnDestroyed(int entityID)
        {
            m_BattleModel.RemoveBattleEntity(entityID);
            m_BattleModel.PlayerEntity = null;

            OnLevelFailed?.Invoke();
        }

        private void Log(string message, bool isImportant = false)
        {
#if LOG_SPAWN

            if (!isImportant)
            {
                UnityEngine.Debug.Log(message);
            }
            else
            {
                UnityEngine.Debug.LogError(message);
            }
#endif
        }
    }
}
