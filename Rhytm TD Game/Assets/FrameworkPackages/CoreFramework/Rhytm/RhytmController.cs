﻿using System.Text;
using UnityEngine;

namespace CoreFramework.Rhytm
{
    public class RhytmController : BaseController
    {
        public System.Action OnStarted;
        public System.Action OnStopped;
        public System.Action<int> OnTick;
        public System.Action<int> OnEventProcessingTick;

        private int m_BPM;
        private bool m_IsStarted;
        private int m_CompletedLoops;
        private double m_NextTickTime;
        private double m_DSPStartTime;
        private double m_TicksSinceStart;
        private double m_LoopPositionInTicks;
        private double m_NextProcessTickTime;

        /// <summary>
        /// Duration of a tick (in seconds)
        /// </summary>
        public double TickDurationSeconds { get; private set; }

        /// <summary>
        /// Ticks passed since start (in ticks)
        /// </summary>
        public int CurrentTick => Mathf.RoundToInt((float)m_TicksSinceStart);

        /// <summary>
        /// Current position in loop (in ticks)
        /// </summary>
        public int LoopPositionTicks => Mathf.RoundToInt((float)m_LoopPositionInTicks);

        /// <summary>
        /// Current position in loop (from 0 to 1)
        /// </summary>
        public double LoopPosition01 => m_LoopPositionInTicks / m_TICKS_PER_LOOP;

        /// <summary>
        /// Time to the next tick (in seconds)
        /// </summary>
        public double TimeToNextTick => m_NextTickTime - AudioSettings.dspTime;

        /// <summary>
        /// Time to the next tick (from 0 to 1)
        /// </summary>
        public double ProgressToNextTick01 => 1 - (TimeToNextTick / TickDurationSeconds);

        /// <summary>
        /// Time since start
        /// </summary>
        public double TimeSinceStart { get; set; }

        /// <summary>
        /// Last stored delta input
        /// </summary>
        public double DeltaInput { get; set; }

        /// <summary>
        /// Last stored input tick result
        /// </summary>
        public EnumsCollection.InputTickResult InputTickResult { get; set; }

        /// <summary>
        /// Amount of seconds on how much command processing tick offsets normal tick
        /// </summary>
        public double ProcessTickDelta { get; private set; }

        private const float m_TICKS_PER_LOOP = 4;
        private const double m_PROCESS_TICK_OFFSET = 0.25;


        public RhytmController(Dispatcher dispatcher) : base(dispatcher)
        {
            m_IsStarted = false;
            m_CompletedLoops = 0;
        }

        public override void InitializeComplete()
        {
            UpdateModel updateModel = Dispatcher.GetModel<UpdateModel>();
            updateModel.OnUpdate += HandleUpdate;
        }

        public void SetBPM(int bpm)
        {
            m_BPM = bpm;
            TickDurationSeconds = 60.0 / m_BPM;
            ProcessTickDelta = TickDurationSeconds * m_PROCESS_TICK_OFFSET;
        }

        public void StartTicking()
        {
            m_DSPStartTime = AudioSettings.dspTime;
            m_NextTickTime = m_DSPStartTime;
            m_TicksSinceStart = 0;

            RefreshTimeSinceStart();

            OnStarted?.Invoke();

            m_IsStarted = true;

            ExecuteTick();
        }

        public float GetTimeToNextTick()
        {
            if (InputTickResult == EnumsCollection.InputTickResult.PreTick)
                return (float)TickDurationSeconds + (float)-DeltaInput;

            return (float)TimeToNextTick;
        }

        public void StopTicking()
        {
            m_IsStarted = false;
            OnStopped?.Invoke();
        }

        public override string ToString()
        {
            StringBuilder str = new StringBuilder(50);
            str.AppendFormat($"TickDuration:        {TickDurationSeconds}  (sec)  \n");
            str.AppendFormat($"Ticks Since Start:   {CurrentTick}      (int)  \n");
            str.AppendFormat($"Ticks Since Start:   {m_TicksSinceStart}    (raw)  \n");
            str.AppendFormat($"Time To next Tick:   {TimeToNextTick}       (sec)  \n");
            str.AppendFormat($"Loop Position        {LoopPositionTicks}    (Tick) \n");
            str.AppendFormat($"Loop PositionAnalog: {LoopPosition01}   (0-1)  \n");
            str.AppendFormat($"TimeToNextTickAnalog {ProgressToNextTick01} (0-1)");

            return str.ToString();
        }


        private void HandleUpdate(float deltaTime)
        {
            if (m_IsStarted)
            {
                RefreshTimeSinceStart();

                //Loops
                if (m_TicksSinceStart >= (m_CompletedLoops + 1) * m_TICKS_PER_LOOP)
                    m_CompletedLoops++;

                //Loop position (in ticks)
                m_LoopPositionInTicks = m_TicksSinceStart - m_CompletedLoops * m_TICKS_PER_LOOP;

                if (TimeToNextTick <= 0)
                {
                    ExecuteTick();
                }

                if (m_NextProcessTickTime - AudioSettings.dspTime <= 0)
                    ExecuteProcessTick();
            }
        }

        private void RefreshTimeSinceStart()
        {
            TimeSinceStart = AudioSettings.dspTime - m_DSPStartTime;
            //m_TicksSinceStart = TimeSinceStart / TickDurationSeconds;
        }


        private void ExecuteTick()
        {
            m_TicksSinceStart++;

            m_NextProcessTickTime = m_NextTickTime + ProcessTickDelta;
            m_NextTickTime = m_NextTickTime + TickDurationSeconds;

            OnTick?.Invoke(CurrentTick);
        }

        private void ExecuteProcessTick()
        {
            m_NextProcessTickTime = double.MaxValue;
            OnEventProcessingTick?.Invoke(CurrentTick);
        }
    }
}
