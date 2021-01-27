using UnityEngine;
using static RhytmTD.Battle.Core.BattleManager;

namespace RhytmTD.Battle.Spawn
{
    public class SpawnController
    {
        private WorldSpawner m_WorldSpawner;
        private Level m_Level;
        private Wave m_CurrentWave;
        private int m_ActionTargetTick;
        private int m_ProcessedWaveTicks;

        public void BuildLevel(WorldSpawner worldSpawner, LevelData l)
        {
            m_WorldSpawner = worldSpawner;

            m_Level = new Level(l.Enemies, l.AttackTicks, l.RestTicks, l.WavesAmount);
            m_CurrentWave = m_Level.GetNextWave();

            m_ActionTargetTick = 1; //TODO: level - delay before start
        }

        public void HandleTick(int ticksSinceStart)
        {
            Debug.Log(ticksSinceStart + " " + m_ActionTargetTick);
            if (m_ActionTargetTick == ticksSinceStart)
            {
                //Spawn wave
                Debug.Log("Spawn wave " + m_CurrentWave.ID);
                m_ProcessedWaveTicks++;

                if (m_ProcessedWaveTicks > m_CurrentWave.DurationAttackTicks)
                {
                    Debug.Log("Finish wave");
                    m_ActionTargetTick += m_CurrentWave.DurationRestTicks;
                    m_CurrentWave = m_Level.GetNextWave();
                    m_ProcessedWaveTicks = 0;
                }
                else
                {
                    //Start idle after spawn
                    m_ActionTargetTick += 2; //TODO: wave - idle after spawn
                    Debug.Log("Wait delay until " + m_ActionTargetTick + ". Remains: " + (m_CurrentWave.DurationAttackTicks - m_ProcessedWaveTicks));
                }

                //m_State = ProcessingStates.IdleBetweenWaves;
            }
        }
    }
}
