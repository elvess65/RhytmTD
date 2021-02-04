//#define LOG_SPAWN
#define SINGLE_SPAWN

using CoreFramework;
using RhytmTD.Battle.Entities;
using RhytmTD.Battle.Entities.Models;
using RhytmTD.Battle.Spawn.Data;
using RhytmTD.Data.Models.DataTableModels;

namespace RhytmTD.Battle.Spawn
{
    /// <summary>
    /// Контроллер создания врагов
    /// </summary>
    public class SpawnController : BaseController
    {
        private WorldSpawner m_WorldSpawner;
        private LevelData m_Level;
        private WaveData m_CurrentWave;
        private BattleModel m_BattleModel;
        private int m_ActionTargetTick;
        private int m_ProcessedChunksAmount;

        public SpawnController(Dispatcher dispatcher) : base(dispatcher)
        {  
        }

        public override void InitializeComplete()
        {
            m_BattleModel = Dispatcher.GetModel<BattleModel>();
        }

        public void BuildLevel(WorldSpawner worldSpawner, WorldDataModel.AreaData areaData, int currentTick)
        {
            m_WorldSpawner = worldSpawner;

            SpawnPlayer();

            m_Level = new LevelData(areaData.ProgressionEnemies,
                                    areaData.ProgressionChunksAmount,
                                    areaData.ProgressionRestTicks,
                                    areaData.ProgressionDelayBetweenChunks,
                                    areaData.WavesAmount,
                                    areaData.DelayBeforeStartLevel);

            //Запланировать тик действия
            m_ActionTargetTick = currentTick + m_Level.DelayBeforeStart;

            //Кешировать следующую волну
            m_CurrentWave = m_Level.GetNextWave();
        }

        public void HandleTick(int ticksSinceStart)
        {
            Log($"Current tick: {ticksSinceStart}. Action at tick {m_ActionTargetTick}");
            if (m_ActionTargetTick == ticksSinceStart)
            {
                SpawnEnemies(m_CurrentWave.EnemiesAmount);
                Log($"Current tick: {ticksSinceStart}. Spawn wave: ID {m_CurrentWave.ID}. Enemies: {m_CurrentWave.EnemiesAmount}", true);

                m_ProcessedChunksAmount++;

                //If all chunks from wave spawned
                if (m_ProcessedChunksAmount >= m_CurrentWave.ChunksAmount)
                {
                    //If level still has waves to spawn
                    if (m_Level.HasWaves)
                    {
                        //Schedule rest delay
                        m_ActionTargetTick += m_CurrentWave.DurationRestTicks;

                        //Get next wave
                        m_CurrentWave = m_Level.GetNextWave();

                        //Reset processed chunks amount
                        m_ProcessedChunksAmount = 0;

                        Log($"Wave finished. Next wave at tick {m_ActionTargetTick}");
                    }
                    else
                    {
                        Log($"All waves spawned");

                        //Stop scheduling tasks
                        m_ActionTargetTick = -1;
                    }
                }
                else
                {
                    //Schedule delay between chunks
                    m_ActionTargetTick += m_CurrentWave.DelayBetweenChunksTicks;

                    Log($"Chunk spawned. Next chunk spawn at {m_ActionTargetTick}. Left {m_CurrentWave.ChunksAmount - m_ProcessedChunksAmount}/{m_CurrentWave.ChunksAmount}");
                }

#if SINGLE_SPAWN
                m_ActionTargetTick = 0;
#endif
            }
        }


        private void SpawnEnemies(int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                BattleEntity enemy = m_WorldSpawner.SpawnEnemy();
                m_BattleModel.AddBattleEntity(enemy);

                if (enemy.HasModule<HealthModule>())
                    enemy.GetModule<HealthModule>().OnDestroyed += EnemyEntity_OnDestroyed;
            }

            m_WorldSpawner.ResetSpawnAreas();
        }

        private void SpawnPlayer()
        {
            BattleEntity entity = m_WorldSpawner.SpawnPlayer();
            m_BattleModel.AddBattleEntity(entity);
            m_BattleModel.PlayerEntity = entity;

            if (entity.HasModule<HealthModule>())
                entity.GetModule<HealthModule>().OnDestroyed += PlayerEntity_OnDestroyed;
        }

        private void EnemyEntity_OnDestroyed(int entityID)
        {
            UnityEngine.Debug.Log("Remove enemy from model: " + entityID);
            m_BattleModel.RemoveBattleEntity(entityID);
        }

        private void PlayerEntity_OnDestroyed(int entityID)
        {
            UnityEngine.Debug.Log("Player was destroyed " + entityID);
            m_BattleModel.RemoveBattleEntity(entityID);
            m_BattleModel.PlayerEntity = null;
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
