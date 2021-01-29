using System.Collections.Generic;
using System.Text;
using RhytmTD.Data;
using UnityEngine;

namespace RhytmTD.Battle.Spawn.Data
{
    public class LevelData
    {
        public int DelayBeforeStart { get; private set; } //Задержка перед началом уровня (в тиках)

        //Кривые распределения
        private ProgressionConfig m_ProgressionEnemies;
        private ProgressionConfig m_ProgressionChunksAmount;
        private ProgressionConfig m_ProgressionRestTicks;
        private ProgressionConfig m_ProgressionDelayBetweenChunks;


        private int m_WavesAmount;

        //Очерель волн для создания
        private Queue<WaveData> m_Waves;

        //Если ли волны в очереди для создания
        public bool HasWaves => m_Waves != null && m_Waves.Count > 0;

        //Количество волн в созданном уровне
        public int WavesAmount => m_Waves.Count;


        public LevelData(ProgressionConfig progressionEnemies,
                         ProgressionConfig progressionChunksAmount,
                         ProgressionConfig progressionRestTicks,
                         ProgressionConfig progressionDelayBetweenChunks,
                         int wavesAmount,
                         int delayBeforeStartLevel)
        {
            m_ProgressionEnemies = progressionEnemies;
            m_ProgressionChunksAmount = progressionChunksAmount;
            m_ProgressionRestTicks = progressionRestTicks;
            m_ProgressionDelayBetweenChunks = progressionDelayBetweenChunks;

            m_WavesAmount = wavesAmount;
            DelayBeforeStart = delayBeforeStartLevel;

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
                int id = i * 100;

                //Количество врагов
                int enemiesAmount = EvaluateFromProgression(m_ProgressionEnemies, waveProgress01);

                //Количество пачек в волне
                int chunksAmount = EvaluateFromProgression(m_ProgressionChunksAmount, waveProgress01);

                //Длительность отдыха после создания всех врагов до начала следующей волны(в тиках)
                int durationRestTicks = EvaluateFromProgression(m_ProgressionRestTicks, waveProgress01);

                //Длительность отдыха между волнами
                int delayBetweenChunks = EvaluateFromProgression(m_ProgressionDelayBetweenChunks, waveProgress01);

                WaveData wave = new WaveData(id, enemiesAmount, chunksAmount, durationRestTicks, delayBetweenChunks);
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
