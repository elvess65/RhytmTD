using RhytmTD.Battle.Spawn.Data;

namespace RhytmTD.Battle.Spawn
{
    public class SpawnController
    {
        private WorldSpawner m_WorldSpawner;
        private LevelData m_Level;
        private WaveData m_CurrentWave;
        private int m_ActionTargetTick;
        private int m_ProcessedChunksAmount;

        public void BuildLevel(WorldSpawner worldSpawner, Core.BattleManager.LevelData levelData)
        {
            m_WorldSpawner = worldSpawner;

            m_Level = new LevelData(levelData.Enemies, levelData.AttackTicks, levelData.RestTicks, levelData.WavesAmount);
            m_ActionTargetTick = m_Level.DelayBeforeStartTicks;
            m_CurrentWave = m_Level.GetNextWave();
        }

        public void HandleTick(int ticksSinceStart)
        {
            if (m_ActionTargetTick == ticksSinceStart)
            {
                m_WorldSpawner.Spawn(m_CurrentWave.EnemiesAmount);

                m_ProcessedChunksAmount++;

                //If all chunks from wave spawned
                if (m_ProcessedChunksAmount > m_CurrentWave.ChunksAmount)
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
                    }
                    else
                    {
                        //Stop scheduling tasks
                        m_ActionTargetTick = -1;
                    }
                }
                else
                {
                    //Schedule delay between chunks
                    m_ActionTargetTick += m_CurrentWave.DelayBetweenChunksTicks;
                }
            }
        }
    }
}
