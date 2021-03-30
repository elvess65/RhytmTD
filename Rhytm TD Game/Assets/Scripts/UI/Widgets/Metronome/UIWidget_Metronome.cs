using CoreFramework.Rhytm;
using CoreFramework.UI.Widget;
using CoreFramework.Utils;
using RhytmTD.UI.Components;
using UnityEngine;

namespace RhytmTD.UI.Widget
{
    /// <summary>
    /// Виджет метронома
    /// </summary>
    public class UIWidget_Metronome : UIWidget
    {
        public Color ColorOutOfRange;
        public Color ColorInRange;

        private RhytmController m_RhytmController;
        private RhytmInputProxy m_RhytmInputProxy;

        [Space(10)]
        [SerializeField] UIComponent_WidgetMetronome_Tick m_TickComponent = null;
        [SerializeField] UIComponent_WidgetMetronome_Arrow[] m_ArrowComponents = null;
   
        private InterpolationData<float> m_LerpData;


        public void Initialize()
        {
            m_RhytmInputProxy = Dispatcher.GetController<RhytmInputProxy>();

            m_RhytmController = Dispatcher.GetController<RhytmController>();
            m_RhytmController.OnTick += TickHandler;
            m_RhytmController.OnEventProcessingTick += ProcessTickHandler;

            m_LerpData = new InterpolationData<float>();

            //Tick Component
            m_TickComponent.Initialize((float)m_RhytmController.TickDurationSeconds / 8);

            //Arrow Components
            for (int i = 0; i < m_ArrowComponents.Length; i++)
            {
                m_ArrowComponents[i].InitializeData(ColorOutOfRange, ColorInRange, m_RhytmInputProxy);
                m_ArrowComponents[i].Initialize();
            }

            InternalInitialize();
        }


        protected override void WidgetUpdate(float deltaTime)
        {
            base.WidgetUpdate(deltaTime);

            ProcessArrowsAnimation();
        }
      
        private void TickHandler(int ticksSinceStart)
        {
            m_TickComponent.StartPlayTickAnimation();
        }

        private void ProcessTickHandler(int ticksSinceStart)
        {
            StartPlayArrowsAnimation();
        }

        private void StartPlayArrowsAnimation()
        {
            for (int i = 0; i < m_ArrowComponents.Length; i++)
                m_ArrowComponents[i].PrepareForInterpolation();

            //Lerp
            m_LerpData.TotalTime = (float)m_RhytmController.TimeToNextTick + (float)m_RhytmController.ProcessTickDelta;
            m_LerpData.Start();
        }

        private void ProcessArrowsAnimation()
        {
            if (m_LerpData.IsStarted)
            {
                //Increment
                m_LerpData.Increment();

                //Process
                for (int i = 0; i < m_ArrowComponents.Length; i++)
                    m_ArrowComponents[i].ProcessInterpolation(m_LerpData.Progress);

                //Overtime
                if (m_LerpData.Overtime())
                {
                    m_LerpData.Stop();

                    for (int i = 0; i < m_ArrowComponents.Length; i++)
                        m_ArrowComponents[i].FinishInterpolation();
                }
            }
        }


        public override void Dispose()
        {
            base.Dispose();

            m_RhytmController.OnTick -= TickHandler;
            m_RhytmController.OnEventProcessingTick -= ProcessTickHandler;
        }
    }
}
