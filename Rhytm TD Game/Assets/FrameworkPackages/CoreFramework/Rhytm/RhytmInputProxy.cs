//#define LOG_ENABLED

namespace CoreFramework.Rhytm
{
    public class RhytmInputProxy : BaseController
    {
        private RhytmController m_RhytmController;
        public double InputPrecious01 { get; private set; }
        private float m_LastInputTime;
        private const float m_TICK_DURATION_REDUCE = 0.4f;

        /// <summary>
        /// Находиться ли текущее состояние времени в окне ввода
        /// </summary>
        public bool IsInUseRange => 1 - m_RhytmController.ProgressToNextTickAnalog < InputPrecious01 ||
                                        m_RhytmController.ProgressToNextTickAnalog < InputPrecious01;


        public RhytmInputProxy(Dispatcher dispatcher) : base(dispatcher)
        {
        }

        public override void InitializeComplete()
        {
            base.InitializeComplete();

            m_RhytmController = Dispatcher.GetController<RhytmController>();
        }

        /// <summary>
        /// Задать точность окна ввода
        /// </summary>
        /// <param name="inputPrecious01">Значение от 0 до 1</param>
        public void SetInputPrecious(double inputPrecious01)
        {
            InputPrecious01 = inputPrecious01;
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

            return UnityEngine.Time.time - m_LastInputTime > m_RhytmController.TickDurationSeconds * m_TICK_DURATION_REDUCE;
        }

        /// <summary>
        /// Находится ли ввод в окне ввода и кеширование результатов в контроллер
        /// </summary>
        public bool IsInputTickValid(double progressToNextTickAnalog)
        {
            bool result = false;

            //Pre tick
            if (progressToNextTickAnalog >= 0.5f)
            {
                m_RhytmController.DeltaInput = -m_RhytmController.TimeToNextTick;
                m_RhytmController.InputTickResult = EnumsCollection.InputTickResult.PreTick;

                result = 1 - progressToNextTickAnalog <= InputPrecious01;
            }
            //Post tick
            else
            {
                m_RhytmController.DeltaInput = m_RhytmController.TickDurationSeconds - m_RhytmController.TimeToNextTick;
                m_RhytmController.InputTickResult = EnumsCollection.InputTickResult.PostTick;

                result = progressToNextTickAnalog <= InputPrecious01;
            }

            Log($"RhytmInputProxy: {m_RhytmController.InputTickResult} : {m_RhytmController.DeltaInput}");

            return result;
        }

        public bool IsInputTickValid() => IsInputTickValid(m_RhytmController.ProgressToNextTickAnalog);

        private void Log(string message)
        {
#if LOG_ENABLED
            UnityEngine.Debug.Log(message);
#endif
        }
    }
}
