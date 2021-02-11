#define LOG_SPAWN
//#define SINGLE_SPAWN

using CoreFramework;
using CoreFramework.Rhytm;
using RhytmTD.Battle.Entities;
using RhytmTD.Battle.Entities.EntitiesFactory;
using RhytmTD.Battle.Entities.Models;
using RhytmTD.Data.Factory;
using RhytmTD.Data.Models;
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

        private SpawnModel m_SpawnModel;
        private BattleModel m_BattleModel;
        private WorldDataModel m_WorldDataModel;
        private AccountDataModel m_AccountDataModel;
        private AccountBaseParamsDataModel m_AccountBaseParamsDataModel;

        private RhytmController m_RhytmController;
        
        private int m_ActionTargetTick;
        private int m_AreaIndex;
        private int m_LevelIndex;
        private int m_WaveIndex;
        private int m_ChunkIndex;

        private AreaData m_CurrentArea => m_WorldDataModel.Areas[m_AreaIndex];
        private LevelDataFactory m_CurrentLevel => m_CurrentArea.LevelsData[m_LevelIndex];
        private LevelDataFactory.WaveDataFactory m_CurrentWave => m_CurrentLevel.Waves[m_WaveIndex];
        private LevelDataFactory.ChunkDataFactory m_CurrentChunk => m_CurrentWave.Chunks[m_ChunkIndex];


        public SpawnController(Dispatcher dispatcher) : base(dispatcher)
        {  
        }

        public override void InitializeComplete()
        {
            m_SpawnModel = Dispatcher.GetModel<SpawnModel>();
            m_BattleModel = Dispatcher.GetModel<BattleModel>();
            m_WorldDataModel = Dispatcher.GetModel<WorldDataModel>();
            m_AccountDataModel = Dispatcher.GetModel<AccountDataModel>();
            m_AccountBaseParamsDataModel = Dispatcher.GetModel<AccountBaseParamsDataModel>();

            m_RhytmController = Dispatcher.GetController<RhytmController>();

            m_RhytmController.OnTick += HandleTick;
        }

        public void BuildLevel()
        {
            m_AreaIndex = m_AccountDataModel.CompletedAreas;
            m_LevelIndex = m_AccountDataModel.CompletedLevels;
            m_WaveIndex = 0;
            m_ChunkIndex = 0;

            //Запланировать тик действия
            m_ActionTargetTick = m_RhytmController.CurrentTick + m_CurrentLevel.DelayBeforeStartLevel;

            //Создать игрока
            SpawnPlayer();
        }


        private void HandleTick(int ticksSinceStart)
        {
            if (m_ActionTargetTick == ticksSinceStart)
            {
                m_SpawnModel.OnAdjustSpawnAreaPosition();
                
                SpawnChunk();

                Log($"Current tick: {ticksSinceStart}. Spawn wave index: {m_WaveIndex}. Enemies: {m_CurrentChunk.EnemiesAmount}", true);

                m_ChunkIndex++;

                //If all chunks from wave spawned
                if (m_ChunkIndex >= m_CurrentWave.Chunks.Count)
                {
                    Log($"All chunks spawned. Increment wave");

                    m_WaveIndex++;

                    //If all waves from level spawned
                    if (m_WaveIndex >= m_CurrentLevel.Waves.Count)
                    {
                        //Reset processed waves amount
                        m_WaveIndex = 0;

                        //Reset processed chunks amount
                        m_ChunkIndex = 0;

                        //Get next level
                        m_LevelIndex++;

                        //If all levels was spawned
                        if (m_LevelIndex >= m_CurrentArea.LevelsData.Length)
                        {
                            //Stop scheduling tasks
                            m_ActionTargetTick = -1;

                            Log("Area is complete");
                        }
                        else 
                        {
                            //Level finished

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

        private void LevelFinishedHandler()
        {
            m_RhytmController.OnTick -= HandleTick;
        }

        private void SpawnChunk()
        {
            //Setup
            EnemyFactorySetup setup = new EnemyFactorySetup(2, m_CurrentChunk.MinDamage, m_CurrentChunk.MaxDamage, m_CurrentChunk.MinHP, m_CurrentChunk.MaxHP);

            //Spawn Entities
            for (int i = 0; i < m_CurrentChunk.EnemiesAmount; i++)
            {
                BattleEntity enemy = m_SpawnModel.OnSpawnEnemyEntity(setup);
                m_BattleModel.AddBattleEntity(enemy);

                if (enemy.HasModule<HealthModule>())
                    enemy.GetModule<HealthModule>().OnDestroyed += EnemyEntity_OnDestroyed;
            }

            //Reset spawn area indexes
            m_SpawnModel.OnResetSpawnAreaUsedAmount();
        }

        private void SpawnPlayer()
        {
            //Setup
            PlayerFactorySetup setup = new PlayerFactorySetup(m_AccountBaseParamsDataModel.FocusSpeed,
                                                              m_AccountBaseParamsDataModel.MinDamage,
                                                              m_AccountBaseParamsDataModel.MaxDamage,
                                                              m_AccountBaseParamsDataModel.Health,
                                                              m_AccountBaseParamsDataModel.MoveSpeed,
                                                              m_AccountBaseParamsDataModel.Mana);

            //Spawn Entity
            BattleEntity entity = m_SpawnModel.OnSpawnPlayerEntity(setup); //m_EntitySpawner.SpawnPlayer(setup);
            m_BattleModel.AddBattleEntity(entity);
            m_BattleModel.PlayerEntity = entity;

            //Cache spawn area position
            m_SpawnModel.OnCacheSpawnAreaPosition(entity.GetModule<TransformModule>());

            entity.GetModule<HealthModule>().OnDestroyed += PlayerEntity_OnDestroyed;
        }

        private void EnemyEntity_OnDestroyed(int entityID)
        {
            m_BattleModel.RemoveBattleEntity(entityID);

            //if (m_BattleModel.BattleEntities.Count == 1 && !m_Level.HasWaves)
            {
              //  OnLevelComplete?.Invoke();
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
