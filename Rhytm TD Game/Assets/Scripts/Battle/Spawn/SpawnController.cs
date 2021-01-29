//#define LOG_SPAWN

using RhytmTD.Battle.Spawn.Data;
using RhytmTD.Data.Models.DataTableModels;

namespace RhytmTD.Battle.Spawn
{
    public class SpawnController
    {
        private WorldSpawner m_WorldSpawner;
        private LevelData m_Level;
        private WaveData m_CurrentWave;
        private int m_ActionTargetTick;
        private int m_ProcessedChunksAmount;

        public void BuildLevel(WorldSpawner worldSpawner, WorldDataModel.AreaData areaData, int currentTick)
        {
            m_WorldSpawner = worldSpawner;

            m_Level = new LevelData(areaData.ProgressionEnemies,
                                    areaData.ProgressionChunksAmount,
                                    areaData.ProgressionRestTicks,
                                    areaData.ProgressionDelayBetweenChunks,
                                    areaData.WavesAmount,
                                    areaData.DelayBeforeStartLevel);

            m_ActionTargetTick = currentTick + m_Level.DelayBeforeStart;
            m_CurrentWave = m_Level.GetNextWave();
        }

        public void HandleTick(int ticksSinceStart)
        {
            Log($"Current tick: {ticksSinceStart}. Action at tick {m_ActionTargetTick}");
            if (m_ActionTargetTick == ticksSinceStart)
            {
                m_WorldSpawner.Spawn(m_CurrentWave.EnemiesAmount);
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
    }
}
