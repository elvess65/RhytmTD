using System.Collections.Generic;
using System.Text;
using RhytmTD.Data;
using UnityEngine;

namespace RhytmTD.Battle.Spawn
{
    public class Level
    {
        //Кривые распределения
        private ProgressionConfig m_ProgressionEnemies;
        private ProgressionConfig m_ProgressionAttackTicks;
        private ProgressionConfig m_ProgressionRestTicks;

        //Количество волн в уровне
        private int m_WavesAmount;

        //Количество пройденных волн
        private int m_CompletedWavesAmount;

        //Волны уровня
        private Queue<Wave> m_Waves;

        //Прогресс прохождения уровня в диапазоне 0-1
        private float m_LevelProgress01 => m_CompletedWavesAmount / (float)m_WavesAmount;


        public Level(ProgressionConfig progressionEnemies, ProgressionConfig progressionAttackTicks, ProgressionConfig progressionRestTicks, int wavesAmount)
        {
            m_ProgressionEnemies = progressionEnemies;
            m_ProgressionAttackTicks = progressionAttackTicks;
            m_ProgressionRestTicks = progressionRestTicks;

            m_WavesAmount = wavesAmount;

            Build();
        }

        private void Build()
        {
            StringBuilder stringBuilder = new StringBuilder();
            m_Waves = new Queue<Wave>();

            for (int i = 0; i < m_WavesAmount; i++)
            {
                float waveProgress01 = (float)i / (m_WavesAmount - 1);

                int id = i + m_CompletedWavesAmount * 100;

                //Количество врагов
                int enemiesAmount = EvaluateFromProgression(m_ProgressionEnemies, waveProgress01);

                //Количество тиков за которое враги должны быть созданы
                int attackTicks = EvaluateFromProgression(m_ProgressionAttackTicks, waveProgress01);

                //Количество тиков после создания всех врагов до начала следующей волны
                int restTicks = EvaluateFromProgression(m_ProgressionRestTicks, waveProgress01);

                Wave wave = new Wave(id, enemiesAmount, attackTicks, restTicks);
                m_Waves.Enqueue(wave);

                stringBuilder.AppendLine(wave.ToString());
            }

            Debug.Log(stringBuilder.ToString());
        }

        public Wave GetNextWave()
        {
            return m_Waves.Dequeue();
        }

        int EvaluateFromProgression(ProgressionConfig progression, float progress01)
        {
            return progression.BaseValue + Mathf.RoundToInt(progression.BaseValue * progression.Evaluate(progress01));
        }
    }
}
