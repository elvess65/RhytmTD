using UnityEngine;

namespace RhytmTD.Data
{
    /// <summary>
    /// Рассчитывает прогрессию необходимого опыта для получения уровня
    /// </summary>
    [System.Serializable]
    public class LevelingProgressionConfig 
    {
        [Tooltip("Кривая прогресии опыта")]
        public AnimationCurve Curve;

        [Tooltip("Общее количество уровней")]
        public int TotalLevels = 10;

        [Tooltip("Общее количество опыта")]
        public int TotalExp = 100;


        private AnimationCurve m_InvertedCurve;


        /// <summary>
        /// Уровень по количеству опыта
        /// </summary>
        public int EvaluateLevel(int expAmount)
        {
            //Прогресс опыта относительно общего количества (expAmount = 15, Exp Range: [0:100], progress = 0.15)
            float expProgress = expAmount / (float)TotalExp;

            //Текущее положение на кривой
            float curveValue = Curve.Evaluate(expProgress);

            //Шаг уровня на кривой (TotalLevels = 20, levelCurveStep = 0.05)
            float levelCurveStep = 1f / TotalLevels;

            //Текущий уровень (curveValue = 0.15, levelCurveStep = 0.05, floatLevel = 3)
            float floatLevel = curveValue / levelCurveStep;
            int level = (int)floatLevel;

            return level;
        }

        /// <summary>
        /// Количество опыта необходимое для уровня
        /// </summary>
        public int EvaluateExpForLevel(int level)
        {
            if (m_InvertedCurve == null)
                m_InvertedCurve = CreateInvertedCurve();

            float levelCurveStep = 1f / TotalLevels;            //Шаг уровня на кривой
            float curveValue = levelCurveStep * level;          //Положение на кривой

            float requExpRaw = m_InvertedCurve.Evaluate(curveValue) * TotalExp;
            int reqExpInt = Mathf.CeilToInt(requExpRaw);

            return reqExpInt;
        }

        /// <summary>
        /// Прогресс достижения уровней (от 0 до 1)
        /// </summary>
        /// <param name="level">Текущий уровень</param>
        public float EvaluateProgress01(int level)
        {
            return (float)level / TotalLevels;
        }


        private AnimationCurve CreateInvertedCurve()
        {
            AnimationCurve invertedConfig = new AnimationCurve();

            float totalTime = Curve.keys[Curve.length - 1].time;
            float sampleX = 0; //The "sample-point"
            float deltaX = 0.01f; //The "sample-delta"
            float lastY = Curve.Evaluate(sampleX);
            while (sampleX < totalTime)
            {
                float y = Curve.Evaluate(sampleX); //The "value"
                float deltaY = y - lastY; //The "value-delta"
                float tangent = deltaX / deltaY;
                Keyframe invertedKey = new Keyframe(y, sampleX, tangent, tangent);
                invertedConfig.AddKey(invertedKey);

                sampleX += deltaX;
                lastY = y;
            }
            for (int i = 0; i < invertedConfig.length; i++)
            {
                invertedConfig.SmoothTangents(i, 0.1f);
            }

            return invertedConfig;
        }
    }
}
