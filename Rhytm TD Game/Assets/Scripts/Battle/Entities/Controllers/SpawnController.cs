//#define LOG_SPAWN
//#define SINGLE_SPAWN

using System.Text;
using CoreFramework;
using CoreFramework.Rhytm;
using RhytmTD.Battle.Entities.EntitiesFactory;
using RhytmTD.Battle.Entities.Models;
using RhytmTD.Data.Factory;
using RhytmTD.Data.Models;
using RhytmTD.Data.Models.DataTableModels;
using UnityEngine;
using static RhytmTD.Data.Models.DataTableModels.WorldDataModel;

namespace RhytmTD.Battle.Entities.Controllers
{
    /// <summary>
    /// Контроллер создания врагов
    /// </summary>
    public class SpawnController : BaseController, IBattleEntityFactory
    {
        private SpawnModel m_SpawnModel;
        private BattleModel m_BattleModel;
        private WorldDataModel m_WorldDataModel;
        private AccountDataModel m_AccountDataModel;
        private AccountBaseParamsDataModel m_AccountBaseParamsDataModel;

        private RhytmController m_RhytmController;
        
        private int m_ActionTargetTick = -1;
        private int m_AreaIndex;
        private int m_LevelIndex;
        private int m_WaveIndex;
        private int m_ChunkIndex;
        private int m_NextSpawnDelay;
        private bool m_IsBattleSpawnFinished = false;

        private AreaData m_CurrentArea => m_WorldDataModel.Areas[m_AreaIndex];
        private LevelDataFactory m_CurrentLevel => m_CurrentArea.LevelsData[m_LevelIndex];
        private LevelDataFactory.WaveDataFactory m_CurrentWave => m_CurrentLevel.Waves[m_WaveIndex];
        private LevelDataFactory.ChunkDataFactory m_CurrentChunk => m_CurrentWave.Chunks[m_ChunkIndex];
        private bool m_AllChunksSpawned => m_ChunkIndex >= m_CurrentWave.Chunks.Count;
        private bool m_AllWavesSpawned => m_WaveIndex >= m_CurrentLevel.Waves.Count;
        private bool m_AllLevelsSpawned => m_LevelIndex >= m_CurrentArea.LevelsData.Length;

        private IBattleEntityFactory m_BattleEntityFactory;
        /// <summary>
        /// Сколько раз использована точка спауна (индекс массива отвечает индексу точки спауна)
        /// </summary>
        private int[] m_SpawnAreaUsedAmount;    
        /// <summary>
        /// На сколько тиков вперед смещена точка спауна (индекс массива отвечает индексу точки спауна)
        /// </summary>
        private int[] m_EnemySpawnAreasOffsetsTicks; 

        /// <summary>
        /// Минимальное количество тиков, на которые будет сдвинута точка спауна при следующей оновлении позиции
        /// </summary>
        private const int m_MIN_SPAWN_AREA_RAW_OFFSET_TICKS_ENEMY = 2;
        /// <summary>
        /// Минимальное количество тиков, на которые будет сдвинута точка спауна при следующей оновлении позиции
        /// </summary>
        private const int m_MAX_SPAWN_AREA_RAW_OFFSET_TICKS_ENEMY = 4;

        /// <summary>
        /// Минимальное количество тиков, на которые будет сдвинута точка спауна при следующей оновлении позиции
        /// </summary>
        private const int m_MIN_SPAWN_AREA_RAW_OFFSET_TICKS_PLAYER = 5;
        /// <summary>
        /// Минимальное количество тиков, на которые будет сдвинута точка спауна при следующей оновлении позиции
        /// </summary>
        private const int m_MAX_SPAWN_AREA_RAW_OFFSET_TICKS_PLAYER = 7;

        /// <summary>
        /// На сколько тиков вперед смещен спаун врага в пределах одной точки спауна относительно предыдущего
        /// </summary>
        private const int m_ENEMY_SPAWN_POINT_USED_OFFSET_TICKS = 3; 


        public SpawnController(Dispatcher dispatcher) : base(dispatcher)
        {
            m_BattleEntityFactory = new DefaultBattleEntityFactory();
        }

        public override void InitializeComplete()
        {
            base.InitializeComplete();

            m_SpawnModel = Dispatcher.GetModel<SpawnModel>();
            m_WorldDataModel = Dispatcher.GetModel<WorldDataModel>();
            m_AccountDataModel = Dispatcher.GetModel<AccountDataModel>();
            m_AccountBaseParamsDataModel = Dispatcher.GetModel<AccountBaseParamsDataModel>();

            m_BattleModel = Dispatcher.GetModel<BattleModel>();
            m_BattleModel.OnBattleStarted += StartSpawnLoop;
            m_BattleModel.OnBattleFinished += StopSpawnLoop;

            m_RhytmController = Dispatcher.GetController<RhytmController>();
            m_RhytmController.OnTick += HandleTick;

            m_SpawnModel.SpawnsInitialized += SpawnAreasInitializedHandler;
        }

 
        public BattleEntity CreatePlayer(int typeID, Vector3 position, Quaternion rotation, float moveSpeed, int health, int minDamage, int maxDamage)
        {
            BattleEntity entity = m_BattleEntityFactory.CreatePlayer(typeID, position, rotation, moveSpeed, health, minDamage, maxDamage);
            m_SpawnModel.OnPlayerCreated?.Invoke(typeID, entity);

            return entity;
        }

        public BattleEntity CreateEnemy(int typeID, Vector3 position, Quaternion rotation, float rotateSpeed, int health, int minDamage, int maxDamage)
        {
            BattleEntity entity = m_BattleEntityFactory.CreateEnemy(typeID, position, rotation, rotateSpeed, health, minDamage, maxDamage);
            m_SpawnModel.OnEnemyCreated?.Invoke(typeID, entity);

            return entity;
        }

        public BattleEntity CreateBullet(int typeID, Vector3 position, Quaternion rotation, float speed, BattleEntity owner)
        {
            BattleEntity entity = m_BattleEntityFactory.CreateBullet(typeID, position, rotation, speed, owner);
            m_SpawnModel.OnBulletCreated?.Invoke(typeID, entity);

            DestroyModule destroyModule = entity.GetModule<DestroyModule>();
            destroyModule.OnDestroyed += BulletDestroyed;

            return entity;
        }


        public void SpawnPlayer()
        {
            float moveSpeed = m_AccountBaseParamsDataModel.MoveSpeedUnitsPerTick * (1 / (float)m_RhytmController.TickDurationSeconds);

            //Spawn Entity
            BattleEntity entity = CreatePlayer(1, m_SpawnModel.PlayerSpawnPosition, Quaternion.identity,
                                               moveSpeed,
                                               m_AccountBaseParamsDataModel.Health,
                                               m_AccountBaseParamsDataModel.MinDamage,
                                               m_AccountBaseParamsDataModel.MaxDamage);

            m_BattleModel.AddBattleEntity(entity);
            m_BattleModel.PlayerEntity = entity;
        }

        public void SpawnEnemy()
        {
            //Span Area indexes
            int randomSpawnAreaIndex = Random.Range(0, m_SpawnModel.EnemySpawnPosition.Length);
            int spawnAreaUsedAmount = m_SpawnAreaUsedAmount[randomSpawnAreaIndex]++;

            float playerSpeed = m_BattleModel.PlayerEntity.GetModule<MoveModule>().CurrentSpeed;
            float worldOffset = playerSpeed * m_ENEMY_SPAWN_POINT_USED_OFFSET_TICKS;
            Vector3 areaUsedOffset = new Vector3(0, 0, worldOffset);
            Vector3 position = m_SpawnModel.EnemySpawnPosition[randomSpawnAreaIndex] + areaUsedOffset * spawnAreaUsedAmount;
            Quaternion rotation = Quaternion.Euler(Quaternion.identity.x, Random.rotation.eulerAngles.y, Quaternion.identity.z);

            BattleEntity enemy = CreateEnemy(1, position, rotation, 2, m_CurrentChunk.MaxHP, m_CurrentChunk.MinDamage, m_CurrentChunk.MaxDamage);
                
            m_BattleModel.AddBattleEntity(enemy);

            if (enemy.HasModule<DestroyModule>())
                enemy.GetModule<DestroyModule>().OnDestroyed += EnemyEntity_OnDestroyed;
        }


        private void HandleTick(int ticksSinceStart)
        {
            if (m_ActionTargetTick == ticksSinceStart)
            {
                //Adjust enemy spawn areas before spawn and schedule next chunk
                PrepareSpawn();

                //Spawn chunk
                SpawnChunk();

                Log($"Current tick: {ticksSinceStart}. Level: {m_LevelIndex}. Wave: {m_WaveIndex}. Chunk: {m_ChunkIndex}. Enemies amount: {m_CurrentChunk.EnemiesAmount}", true);

                m_ChunkIndex++;

                if (m_AllChunksSpawned)
                {
                    Log($"All chunks spawned. Wave {m_WaveIndex} finished");

                    m_WaveIndex++;

                    //All waves from level spawned
                    if (m_AllWavesSpawned)
                    {
                        m_LevelIndex++;

                        //If all levels was spawned
                        if (m_AllLevelsSpawned)
                        {
                            FinishBattle();
                        }
                        else
                        {
                            ScheduleNextLevel();
                            FinishBattle(); //TODO: DEBUG
                        }
                    }
                    else
                    {
                        ScheduleNextWave();
                    }
                }
                else if (m_ChunkIndex < m_CurrentWave.Chunks.Count)
                {
                    ScheduleNextChunk();
                }
                
#if SINGLE_SPAWN
                m_ActionTargetTick = 0;
#endif
            }
        }

 
        private void FinishBattle()
        {
            //Stop scheduling tasks
            m_ActionTargetTick = -1;

            //Al enemies spawned
            m_IsBattleSpawnFinished = true;

            Log("Battle is complete");
        }

        private void ScheduleNextLevel()
        {
            //Reset processed waves amount
            m_WaveIndex = 0;

            //Reset processed chunks amount
            m_ChunkIndex = 0;

            //Запланировать тик действия
            m_ActionTargetTick += m_CurrentLevel.DelayBeforeStartLevel;

            Log($"All waves spawned. Increment level. Next action at tick: {m_ActionTargetTick}");
        }

        private void ScheduleNextWave()
        {
            //Schedule rest delay
            m_ActionTargetTick += m_CurrentWave.DurationRestTicks + m_NextSpawnDelay;

            //Reset processed chunks amount
            m_ChunkIndex = 0;

            Log($"Wave finished. Next wave at tick {m_ActionTargetTick}");
        }

        private void ScheduleNextChunk()
        {
            //Schedule delay between chunks
            m_ActionTargetTick = m_RhytmController.CurrentTick + m_NextSpawnDelay;

            Log($"Chunk spawned. Next chunk spawn at {m_ActionTargetTick}. Left {m_CurrentWave.Chunks.Count - m_ChunkIndex}/{m_CurrentWave.Chunks.Count}");
        }


        private void StartSpawnLoop()
        {
            m_AreaIndex = m_AccountDataModel.CompletedAreas;
            m_LevelIndex = m_AccountDataModel.CompletedLevels;
            m_WaveIndex = 0;
            m_ChunkIndex = 0;

            m_IsBattleSpawnFinished = false;

            //Запланировать тик действия
            m_ActionTargetTick = m_RhytmController.CurrentTick + m_CurrentLevel.DelayBeforeStartLevel;
        }

        private void StopSpawnLoop(bool isSuccess)
        {
            FinishBattle();
        }

        //TODO: OnEnemy destroyed or TakeDamag - if no enemy execute next action on next tick if not all chunks spawned

        private void SpawnChunk()
        {
            //Spawn Entities
            for (int i = 0; i < m_CurrentChunk.EnemiesAmount; i++)
            {
                SpawnEnemy();
            }
        }

        private void PrepareSpawn()
        {
            //Recalculate spawn area offsets
            int offsetTicks = CalculateSpawnAreaOffsetTick();

            m_NextSpawnDelay = offsetTicks / 2;

            for (int i = 0; i < m_SpawnModel.EnemySpawnPosition.Length; i++)
                m_EnemySpawnAreasOffsetsTicks[i] = offsetTicks;

            //Reset spawn area indexes
            for (int i = 0; i < m_SpawnAreaUsedAmount.Length; i++)
                m_SpawnAreaUsedAmount[i] = 0;

            //Offset each spawn point by amount of ticks
            TransformModule playerTransformModule = m_BattleModel.PlayerEntity.GetModule<TransformModule>();
            float playerSpeed = m_BattleModel.PlayerEntity.GetModule<MoveModule>().CurrentSpeed;
            Vector3 anchorPosition = playerTransformModule.Position;
                                     
            for (int i = 0; i < m_SpawnModel.EnemySpawnPosition.Length; i++)
            {
                float worldOffset = anchorPosition.z + playerSpeed * m_EnemySpawnAreasOffsetsTicks[i];
                m_SpawnModel.EnemySpawnPosition[i] = new Vector3(m_SpawnModel.EnemySpawnPosition[i].x, m_SpawnModel.EnemySpawnPosition[i].y, worldOffset);
            }
        }

        private int CalculateSpawnAreaOffsetTick()
        {
            DamageModule playerDamageModule = m_BattleModel.PlayerEntity.GetModule<DamageModule>();
            int playerAverageDamage = (playerDamageModule.MinDamage + playerDamageModule.MaxDamage) / 2;

            int extraBufferPercent = 200;

            int ticks2DestroyExistingEnemies = CalculateApproxTick2DestroyExistingEnemies(playerAverageDamage);
            int ticks2DestroyNextChunk = CalculateApproxTick2DestroyNextChunk(playerAverageDamage);
            int extraTicks = Mathf.CeilToInt((ticks2DestroyNextChunk * extraBufferPercent) / 100f);
            int totalRawTicks = ticks2DestroyExistingEnemies + ticks2DestroyNextChunk;
            int totalTicks = totalRawTicks + extraTicks;
            
            Debug.Log($"t2DExisting {ticks2DestroyExistingEnemies} t2DNext {ticks2DestroyNextChunk} TotalRaw {totalRawTicks} " +
                $"Extra {extraTicks} Total {totalTicks} NextAfter {totalTicks / 2}");

            return totalTicks;

        }

        private int GetEnemiesAmount()
        {
            int enemiesAmount = 0;

            foreach (BattleEntity entity in m_BattleModel.BattleEntities)
            {
                if (entity.ID == m_BattleModel.ID || !entity.HasModule<EnemyBehaviourTag>() || entity.HasModule<PredictedDestroyedTag>())
                    continue;

                enemiesAmount++;
            }

            return enemiesAmount;
        }

        private BattleEntity GetFarthestEnemy(TransformModule transformModule)
        {
            BattleEntity result = null;
            float dist2Enemy = float.MinValue;

            foreach (BattleEntity entity in m_BattleModel.BattleEntities)
            {
                if (entity.ID == m_BattleModel.ID || !entity.HasModule<EnemyBehaviourTag>() || entity.HasModule<PredictedDestroyedTag>())
                    continue;

                float dist = entity.GetModule<TransformModule>().Position.z - transformModule.Position.z;
                if (dist > dist2Enemy)
                {
                    dist2Enemy = dist;
                    result = entity;
                }
            }

            return result;
        }

        private int CalculateApproxTick2DestroyNextChunk(int playerAverageDamage)
        {
            int maxChunkHP = m_CurrentChunk.MaxHP * m_CurrentChunk.EnemiesAmount;

            return Mathf.CeilToInt((float)maxChunkHP / playerAverageDamage);
        }

        private int CalculateApproxTick2DestroyExistingEnemies(int playerAverageDamage)
        {
            int enemiesComulatedCurrentHealth = 0;

            //Collect current enemies health
            foreach (BattleEntity entity in m_BattleModel.BattleEntities)
            {
                if (entity.ID == m_BattleModel.ID || !entity.HasModule<EnemyBehaviourTag>() || entity.HasModule<PredictedDestroyedTag>())
                    continue;

                //Comulated health
                HealthModule healthModule = entity.GetModule<HealthModule>();
                enemiesComulatedCurrentHealth += healthModule.CurrentHealth;
            }

            return Mathf.CeilToInt((float)enemiesComulatedCurrentHealth / playerAverageDamage);
        }

        private void SpawnAreasInitializedHandler()
        {
            m_SpawnAreaUsedAmount = new int[m_SpawnModel.EnemySpawnPosition.Length];
            m_EnemySpawnAreasOffsetsTicks = new int[m_SpawnModel.EnemySpawnPosition.Length];
        }


        private void EnemyEntity_OnDestroyed(BattleEntity entity)
        {
            m_BattleModel.RemoveBattleEntity(entity.ID);

            if (GetEnemiesAmount() == 0 && m_IsBattleSpawnFinished)
            {
                m_BattleModel.OnBattleFinished?.Invoke(true);
            }
        }

        private void BulletDestroyed(BattleEntity battleEntity)
        {
            m_BattleModel.RemoveBattleEntity(battleEntity.ID);
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
