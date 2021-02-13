namespace CoreFramework.Rhytm
{
    public class RhytmInputProxy : BaseController
    {
        private static RhytmInputProxy m_Instance;

        private double m_InputPrecious01;
        private float m_LastInputTime;
        private const float m_TICK_DURATION_REDUCE = 0.4f;

        /// <summary>
        /// Находиться ли текущее состояние времени в окне ввода
        /// </summary>
        [System.Obsolete("Dont forget to remove this")]
        public static bool IsInUseRange => 1 - RhytmController.GetInstance().ProgressToNextTickAnalog < m_Instance.m_InputPrecious01 ||
                                               RhytmController.GetInstance().ProgressToNextTickAnalog < m_Instance.m_InputPrecious01;


        public RhytmInputProxy(Dispatcher dispatcher) : base(dispatcher)
        {
            if (m_Instance == null)
                m_Instance = this;
        }

        /// <summary>
        /// Задать точность окна ввода
        /// </summary>
        /// <param name="inputPrecious01">Значение от 0 до 1</param>
        public void SetInputPrecious(double inputPrecious01)
        {
            m_InputPrecious01 = inputPrecious01;
        }

        /// <summary>
        /// Зарегистрировать время последнего ввода (предотвращение закликивания)
        /// </summary>
        public void RegisterInput()
        {
            m_LastInputTime = UnityEngine.Time.time;
        }

        /// <summary>
        /// Разрешен ли ввод на этом тике 
        /// </summary>
        public bool IsInputAllowed()
        {
            //Время с предыдущего тика сравнивается с временем тика уменьшеным на константу

            return UnityEngine.Time.time - m_LastInputTime > RhytmController.GetInstance().TickDurationSeconds * m_TICK_DURATION_REDUCE;
        }

        /// <summary>
        /// Находится ли ввод в окне ввода и кеширование результатов в контроллер
        /// </summary>
        /// <returns></returns>
        public bool IsInputTickValid()
        {
            double progressToNextTickAnalog = RhytmController.GetInstance().ProgressToNextTickAnalog;

            //Pre tick
            if (progressToNextTickAnalog >= 0.5f)
            {
                RhytmController.GetInstance().DeltaInput = -RhytmController.GetInstance().TimeToNextTick;
                RhytmController.GetInstance().InputTickResult = EnumsCollection.InputTickResult.PreTick;

                //UnityEngine.Debug.Log("RhytmInputProxy: Pre Tick: " + RhytmController.GetInstance().DeltaInput);

                return 1 - progressToNextTickAnalog <= m_InputPrecious01;
            }
            //Post tick
            else
            {
                RhytmController.GetInstance().DeltaInput = RhytmController.GetInstance().TickDurationSeconds - RhytmController.GetInstance().TimeToNextTick;
                RhytmController.GetInstance().InputTickResult = EnumsCollection.InputTickResult.PostTick;

                //UnityEngine.Debug.Log("RhytmInputProxy: Post Tick: " + RhytmController.GetInstance().DeltaInput);

                return progressToNextTickAnalog <= m_InputPrecious01;
            }
        }
    }
}
