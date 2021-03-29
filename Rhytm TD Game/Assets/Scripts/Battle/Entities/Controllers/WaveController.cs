//#define LOG_SPAWN
//#define SINGLE_SPAWN
//#define DISABLE_SPAWN

#if DEBUG_SPAWN
using CoreFramework.Input;
#endif
using System.Collections.Generic;
using CoreFramework;
using CoreFramework.Rhytm;
using RhytmTD.Battle.Entities.Models;
using RhytmTD.Data.Factory;
using RhytmTD.Data.Models;
using RhytmTD.Data.Models.DataTableModels;
using UnityEngine;
using static RhytmTD.Data.Models.DataTableModels.WorldDataModel;

namespace RhytmTD.Battle.Entities.Controllers
{
    /// <summary>
    /// Controlls spawning waves
    /// </summary>
    public class WaveController : BaseController
    {
#if DEBUG_SPAWN
        private InputModel m_InputModel;
#endif
        private SpawnModel m_SpawnModel;
        private BattleModel m_BattleModel;
        private SpellBookModel m_SpellBookModel;
        private WorldDataModel m_WorldDataModel;
        private AccountDataModel m_AccountDataModel;
        private PlayerRhytmInputHandleModel m_PlayerRhytmInputHandleModel;

        private RhytmController m_RhytmController;
        private SolidEntitySpawnController m_SolidEntitySpawnController;

        private int m_CurDynamicDifficultyReduceOffset;
        private int m_ActionTargetTick = -1;
        private int m_AreaIndex;
        private int m_LevelIndex;
        private int m_WaveIndex;
        private int m_ChunkIndex;
        private int m_NextSpawnDelay;
        private int m_SpellbookTick;

        private AreaData m_CurrentArea => m_WorldDataModel.Areas[m_AreaIndex];
        private LevelDataFactory m_CurrentLevel => m_CurrentArea.LevelsData[m_LevelIndex];
        private LevelDataFactory.WaveDataFactory m_CurrentWave => m_CurrentLevel.Waves[m_WaveIndex];
        private LevelDataFactory.ChunkDataFactory m_CurrentChunk => m_CurrentWave.Chunks[m_ChunkIndex];
        private bool m_AllChunksSpawned => m_ChunkIndex >= m_CurrentWave.Chunks.Count;
        private bool m_AllWavesSpawned => m_WaveIndex >= m_CurrentLevel.Waves.Count;
        private bool m_AllLevelsSpawned => m_LevelIndex >= m_CurrentArea.LevelsData.Length;

        /// <summary>
        /// На сколько тиков вперед смещена точка спауна (индекс массива отвечает индексу точки спауна)
        /// </summary>
        private int[] m_EnemySpawnAreasOffsetsTicks;
        /// <summary>
        /// Матрица точек спауна
        /// </summary>
        private List<(int column, int row)> m_AvailableESPMatrixIndexes;

        /// <summary>
        /// На сколько тиков смещен спаун следующего чанка относительно самого дальнего текущего врага
        /// </summary>
        private const int m_FARTHEST_ENEMY_OFFSET_TICKS = 2;

        /// <summary>
        /// На сколько тиков вперед смещен спаун врага на следующем ряду относительно предыдущего
        /// </summary>
        private const int m_ESP_MATRIX_ROW_OFFSET_TICKS = 2;


        public WaveController(Dispatcher dispatcher) : base(dispatcher)
        {
        }

        public override void InitializeComplete()
        {
            base.InitializeComplete();

            m_SpawnModel = Dispatcher.GetModel<SpawnModel>();
            m_BattleModel = Dispatcher.GetModel<BattleModel>();
            m_SpellBookModel = Dispatcher.GetModel<SpellBookModel>();

            m_WorldDataModel = Dispatcher.GetModel<WorldDataModel>();
            m_AccountDataModel = Dispatcher.GetModel<AccountDataModel>();
            m_PlayerRhytmInputHandleModel = Dispatcher.GetModel<PlayerRhytmInputHandleModel>();

            m_RhytmController = Dispatcher.GetController<RhytmController>();
            m_SolidEntitySpawnController = Dispatcher.GetController<SolidEntitySpawnController>();

            m_BattleModel.OnBattleStarted += StartSpawnLoop;
            m_BattleModel.OnBattleFinished += BattleFinishedHandler;

            m_SpellBookModel.OnSpellbookOpened += SpellBookOpenedHandler;
            m_SpellBookModel.OnSpellbookClosed += SpellBookClosedAndPostUsedHandler;
            m_SpellBookModel.OnSpellbookPostUsed += SpellBookClosedAndPostUsedHandler;

            m_RhytmController.OnTick += HandleTick;
            m_SpawnModel.OnSpawnPointsInitialized += SpawnAreasInitializedHandler;            
            m_PlayerRhytmInputHandleModel.OnDDRPInputCounterChanged += DDRPInputCounterChangedHandler;            

#if DEBUG_SPAWN
            m_InputModel = Dispatcher.GetModel<InputModel>();
            m_InputModel.OnKeyDown += HandleDebugInput;
#endif
        }

        private void SpawnEnemy(int columnIndex, int rowIndex, float playerSpeed)
        {
            //Spawn point matrix row offset
            float worldRowOffset = rowIndex * playerSpeed * m_ESP_MATRIX_ROW_OFFSET_TICKS;

            //Get column position and add row offset
            Vector3 position = m_SpawnModel.EnemySpawnPosition[columnIndex] + new Vector3(0, 0, worldRowOffset);

            //Random rotaion
            Quaternion rotation = Quaternion.Euler(Quaternion.identity.x, Random.rotation.eulerAngles.y, Quaternion.identity.z);

            int typeID = 1;
            float rotationSpeed = 2;

            //Spawn Entity
            BattleEntity enemy = m_SolidEntitySpawnController.SpawnEnemy(typeID,
                                                                         position,
                                                                         rotation, rotationSpeed,
                                                                         m_CurrentChunk.MaxHP,
                                                                         m_CurrentChunk.MinDamage,
                                                                         m_CurrentChunk.MaxDamage);

            m_BattleModel.AddBattleEntity(enemy);

            if (enemy.HasModule<DestroyModule>())
                enemy.GetModule<DestroyModule>().OnDestroyed += EnemyEntityDestroyedHandler;
        }

        private void HandleTick(int ticksSinceStart)
        {
#if !DISABLE_SPAWN
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
                        //m_LevelIndex++;

                        //If all levels was spawned
                        if (m_AllLevelsSpawned)
                        {
                            StopSpawnLoop();
                        }
                        else
                        {
                            ScheduleNextLevel();
                            StopSpawnLoop(); //TODO: DEBUG
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
#endif
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

            m_SpawnModel.IsBattleSpawnFinished = false;

            //Запланировать тик действия
            m_ActionTargetTick = m_RhytmController.CurrentTick + m_CurrentLevel.DelayBeforeStartLevel;
        }

        private void StopSpawnLoop()
        {
            //Stop scheduling tasks
            m_ActionTargetTick = -1;

            //Al enemies spawned
            m_SpawnModel.IsBattleSpawnFinished = true;

            Log("Battle is complete");
        }


        private void SpawnChunk()
        {
            //Player speed
            float playerSpeed = m_BattleModel.PlayerEntity.GetModule<MoveModule>().CurrentSpeed;

            //Amount of enemies in current chunk
            int enemiesAmount = m_CurrentWave.EnemiesAmount;

            //Max row index
            int maxESPMatrixRowIndex = Mathf.FloorToInt(enemiesAmount / m_SpawnModel.EnemySpawnPosition.Length) + 1;

            //Matrix size
            int columnsAmount = m_SpawnModel.EnemySpawnPosition.Length;
            int rowsAmount = maxESPMatrixRowIndex + 1;

            //Fill matrix array
            CalculateSpawnMatrixArray(columnsAmount, rowsAmount);

            //Spawn Entities
            for (int i = 0; i < enemiesAmount; i++)
            {
                //Max available row index for current enemy
                int maxAllowedRowsIndex = i < maxESPMatrixRowIndex ? i % maxESPMatrixRowIndex : maxESPMatrixRowIndex;

                //Random matrix elements
                int rndMatrixElement = Random.Range(0, m_SpawnModel.EnemySpawnPosition.Length * (maxAllowedRowsIndex + 1) - i);

                //Random column and row
                int rndColumn = m_AvailableESPMatrixIndexes[rndMatrixElement].column;
                int rndRow = m_AvailableESPMatrixIndexes[rndMatrixElement].row;

                //Remove random element to prevent spawn in the same pos
                m_AvailableESPMatrixIndexes.RemoveAt(rndMatrixElement);

                //Spawn
                SpawnEnemy(rndColumn, rndRow, playerSpeed);
            }
        }

        private void PrepareSpawn()
        {
            TransformModule playerTransformModule = m_BattleModel.PlayerEntity.GetModule<TransformModule>();
            MoveModule playerMoveModule = m_BattleModel.PlayerEntity.GetModule<MoveModule>();

            //Recalculate spawn area offset
            int spawnOffsetTicks = CalculateSpawnAreaOffsetTick();

            //Distance to the farthest enemy to prevent enemies position overlapping
            int ticks2FarthestEnemy = CalculateTicks2FarthestEnemy(playerTransformModule, playerMoveModule);

            //Adjust if calculated spawn supposed to be spawned closer than farthest enemy - 
            if (spawnOffsetTicks <= ticks2FarthestEnemy)
            {
                Debug.Log("Offset is less than distance to farthest enemy. Adjusting...");
                spawnOffsetTicks = ticks2FarthestEnemy + m_FARTHEST_ENEMY_OFFSET_TICKS;
            }
            else
            {
                //Debug.Log($"Offset {spawnOffsetTicks} is more then d2fe {ticks2FarthestEnemy}");
                spawnOffsetTicks = Mathf.Max(spawnOffsetTicks - ticks2FarthestEnemy, ticks2FarthestEnemy + m_FARTHEST_ENEMY_OFFSET_TICKS);
            }

            //Debug.Log($"Spawn offset: {spawnOffsetTicks}");

            m_NextSpawnDelay = spawnOffsetTicks / 2;

            for (int i = 0; i < m_SpawnModel.EnemySpawnPosition.Length; i++)
                m_EnemySpawnAreasOffsetsTicks[i] = spawnOffsetTicks;

            //Offset each spawn point by amount of ticks                         
            for (int i = 0; i < m_SpawnModel.EnemySpawnPosition.Length; i++)
            {
                float worldOffset = playerTransformModule.Position.z + playerMoveModule.CurrentSpeed * m_EnemySpawnAreasOffsetsTicks[i];
                m_SpawnModel.EnemySpawnPosition[i] = new Vector3(m_SpawnModel.EnemySpawnPosition[i].x,
                                                                 m_SpawnModel.EnemySpawnPosition[i].y,
                                                                 worldOffset);
            }
        }


        private int CalculateSpawnAreaOffsetTick()
        {
            //Recomended damage decreased by difficulty percent
            int targetDmg = Mathf.RoundToInt(m_CurrentLevel.RecomendedAverageDmg * (m_CurDynamicDifficultyReduceOffset / 100f));

            //Approx amount of ticks to destroy existing enemies
            int ticks2DestroyExistingEnemies = CalculateApproxTick2DestroyExistingEnemies(targetDmg);

            //Approx amount of ticks to destroy next spawned chunk
            int ticks2DestroyNextChunk = CalculateApproxTick2DestroyNextChunk(targetDmg);

            //Approx ticks to destroy all enemies
            int totalTicks = ticks2DestroyExistingEnemies + ticks2DestroyNextChunk;

            if (totalTicks < m_CurrentLevel.MinSpawnTicksOffset)
                totalTicks = m_CurrentLevel.MinSpawnTicksOffset;

            //Debug.Log($"rD: {m_CurrentLevel.RecomendedAverageDmg} tD: {targetDmg} " +
            //    $"t2DExisting: {ticks2DestroyExistingEnemies} t2DNext: {ticks2DestroyNextChunk} Total: {totalTicks}");

            return totalTicks;

        }

        private int CalculateApproxTick2DestroyNextChunk(int targetDmg)
        {
            int maxChunkHP = m_CurrentChunk.MaxHP * m_CurrentChunk.EnemiesAmount;

            return Mathf.CeilToInt((float)maxChunkHP / targetDmg);
        }

        private int CalculateApproxTick2DestroyExistingEnemies(int targetDmg)
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

            return Mathf.CeilToInt((float)enemiesComulatedCurrentHealth / targetDmg);
        }

        private int CalculateTicks2FarthestEnemy(TransformModule playerTransformModule, MoveModule playerMoveModule)
        {
            int ticks = 0;
            BattleEntity farthestEnemyEntity = GetFarthestEnemy(playerTransformModule);

            if (farthestEnemyEntity != null)
            {
                TransformModule enemyTransformModule = farthestEnemyEntity.GetModule<TransformModule>();

                float worldDistance = Vector3.Distance(enemyTransformModule.Position, playerTransformModule.Position);
                ticks = Mathf.CeilToInt(worldDistance / playerMoveModule.CurrentSpeed);

                //Debug.Log($"D2fE: WD {worldDistance} TD {ticks}");
            }

            return ticks;
        }

        private void CalculateSpawnMatrixArray(int columnsAmount, int rowsAmount)
        {
            m_AvailableESPMatrixIndexes.Clear();
            int matrixSize = columnsAmount * rowsAmount;

            for (int i = 0; i < matrixSize; i++)
            {
                int column = i % columnsAmount;
                int row = i / columnsAmount;

                m_AvailableESPMatrixIndexes.Add((column, row));
            }
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


        private void SpawnAreasInitializedHandler()
        {
            m_CurDynamicDifficultyReduceOffset = m_CurrentLevel.InitDDRP;
            m_EnemySpawnAreasOffsetsTicks = new int[m_SpawnModel.EnemySpawnPosition.Length];
            m_AvailableESPMatrixIndexes = new List<(int, int)>();
        }

        private void EnemyEntityDestroyedHandler(BattleEntity entity)
        {
            m_BattleModel.RemoveBattleEntity(entity.ID);
            m_SpawnModel.OnEnemyRemoved(entity);
        }

        private void BattleFinishedHandler(bool isSuccess)
        {
            StopSpawnLoop();
        }


        
        private void DDRPInputCounterChangedHandler(int curDDRPInputCounter)
        {
            m_CurDynamicDifficultyReduceOffset = (int)Mathf.Lerp(m_CurrentLevel.MinDDRP,
                                                                 m_CurrentLevel.MaxDDRP,
                                                                 (float)curDDRPInputCounter / m_CurrentLevel.StepsToChangeDDRP);
        }

        private void SpellBookOpenedHandler()
        {
            m_RhytmController.OnTick -= HandleTick;

            m_SpellbookTick = m_RhytmController.CurrentTick;
        }

        private void SpellBookClosedAndPostUsedHandler()
        {
            m_RhytmController.OnTick += HandleTick;

            if (m_ActionTargetTick >= 0)
            {
                int ticksInSpellBook = m_RhytmController.CurrentTick - m_SpellbookTick;

                m_ActionTargetTick += ticksInSpellBook;

                if (m_ActionTargetTick == m_RhytmController.CurrentTick)
                    m_ActionTargetTick++;
            }
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

#if DEBUG_SPAWN
        private void HandleDebugInput(KeyCode keyCode)
        {
            if (keyCode != KeyCode.D)
                return;

            //Adjust enemy spawn areas before spawn and schedule next chunk
            PrepareSpawn();

            //Spawn chunk
            SpawnChunk();
        }
#endif
    }
}
