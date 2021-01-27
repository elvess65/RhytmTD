using System.Collections.Generic;
using System.Text;
using RhytmTD.Data;
using UnityEngine;

namespace RhytmTD.Battle.Spawn.Data
{
    public class LevelData
    {
        public int DelayBeforeStartTicks => 1; //Задержка перед началом уровня (в тиках)

        //Кривые распределения
        private ProgressionConfig m_ProgressionEnemies;
        private ProgressionConfig m_ProgressionChunksAmount;
        private ProgressionConfig m_ProgressionRestTicks;

        //Количество волн в уровне
        private int m_WavesAmount;

        //Очерель волн для создания
        private Queue<WaveData> m_Waves;

        //Прогресс прохождения уровня в диапазоне 0-1
        private float m_LevelProgress01 => m_CompletedWavesAmount / (float)m_WavesAmount;

        //Количество пройденных волн (все враги волны уничтожены)
        private int m_CompletedWavesAmount { get; set; }

        //Если ли волны в очереди для создания
        public bool HasWaves => m_Waves != null && m_Waves.Count > 0;


        public LevelData(ProgressionConfig progressionEnemies, ProgressionConfig progressionChunksAmount, ProgressionConfig progressionRestTicks, int wavesAmount)
        {
            m_ProgressionEnemies = progressionEnemies;
            m_ProgressionChunksAmount = progressionChunksAmount;
            m_ProgressionRestTicks = progressionRestTicks;

            m_WavesAmount = wavesAmount;

            Build();
        }

        public WaveData GetNextWave()
        {
            return m_Waves.Dequeue();
        }


        private void Build()
        {
            StringBuilder stringBuilder = new StringBuilder();
            m_Waves = new Queue<WaveData>();

            for (int i = 0; i < m_WavesAmount; i++)
            {
                //Интерполировання позиция волны относительно уровня
                float waveProgress01 = (float)i / (m_WavesAmount - 1);

                //ИД волны
                int id = i + m_CompletedWavesAmount * 100;

                //Количество врагов
                int enemiesAmount = EvaluateFromProgression(m_ProgressionEnemies, waveProgress01);

                //Количество пачек в волне
                int chunksAmount = EvaluateFromProgression(m_ProgressionChunksAmount, waveProgress01);

                //Длительность отдыха после создания всех врагов до начала следующей волны(в тиках)
                int durationRestTicks = EvaluateFromProgression(m_ProgressionRestTicks, waveProgress01);

                WaveData wave = new WaveData(id, enemiesAmount, chunksAmount, durationRestTicks);
                m_Waves.Enqueue(wave);

                stringBuilder.AppendLine(wave.ToString());
            }

            Debug.Log(stringBuilder.ToString());
        }

        private int EvaluateFromProgression(ProgressionConfig progression, float progress01)
        {
            return progression.BaseValue + Mathf.RoundToInt(progression.BaseValue * progression.Evaluate(progress01));
        }
    }
}
