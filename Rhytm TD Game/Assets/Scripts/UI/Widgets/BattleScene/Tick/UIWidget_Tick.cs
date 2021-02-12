using CoreFramework.Rhytm;
using CoreFramework.Utils;
using RhytmTD.UI.Components;
using UnityEngine;

namespace RhytmTD.UI.Widget
{
    /// <summary>
    /// Виджет отображения состояния тиков
    /// </summary>
    public class UIWidget_Tick : UIWidget
    {
        private RhytmController m_RhytmController;

        [Space(10)]
        [SerializeField] UIComponent_TickWidget_Tick m_Tick;
        [SerializeField] UIComponent_TickWidget_Arrow[] m_TickArrows;
   
        private InterpolationData<float> m_LerpData;


        public void Initialize(float tickDuration)
        {
            m_RhytmController = Dispatcher.GetController<RhytmController>();

            //Lerp
            m_LerpData = new InterpolationData<float>();

            //Tick
            m_Tick.Initialize(tickDuration);

            //Arrows
            for (int i = 0; i < m_TickArrows.Length; i++)
                m_TickArrows[i].Initialize();

            InternalInitialize();
        }

        public void StartPlayTickAnimation()
        {
            m_Tick.StartPlayTickAnimation();
        }

        public void StartPlayArrowsAnimation()
        {
            //Arrows
            for (int i = 0; i < m_TickArrows.Length; i++)
                m_TickArrows[i].PrepareForInterpolation();

            //Lerp
            m_LerpData.TotalTime = (float)m_RhytmController.TimeToNextTick + (float)m_RhytmController.ProcessTickDelta;
            m_LerpData.Start();
        }


        protected override void WidgetUpdate(float deltaTime)
        {
            base.WidgetUpdate(deltaTime);

            PlayArrowsAnimation();
        }

        private void PlayArrowsAnimation()
        {
            if (m_LerpData.IsStarted)
            {
                //Increment
                m_LerpData.Increment();

                //Process
                for (int i = 0; i < m_TickArrows.Length; i++)
                    m_TickArrows[i].ProcessInterpolation(m_LerpData.Progress);

                //Overtime
                if (m_LerpData.Overtime())
                {
                    m_LerpData.Stop();

                    for (int i = 0; i < m_TickArrows.Length; i++)
                        m_TickArrows[i].FinishInterpolation();
                }
            }
        }
    }
}
