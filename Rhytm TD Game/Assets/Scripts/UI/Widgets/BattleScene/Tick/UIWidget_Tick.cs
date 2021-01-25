using CoreFramework.Abstract;
using CoreFramework.Rhytm;
using CoreFramework.Utils;
using RhytmTD.UI.Components;
using UnityEngine;

namespace RhytmTD.UI.Widget
{
    /// <summary>
    /// Виджет отображения состояния тиков
    /// </summary>
    public class UIWidget_Tick : UIWidget, iUpdatable
    {
        [Space(10)]
        [SerializeField] UIComponent_TickWidget_Tick m_Tick;
        [SerializeField] UIComponent_TickWidget_Arrow[] m_TickArrows;
   
        private InterpolationData<float> m_LerpData;


        public void Initialize(float tickDuration)
        {
            //Lerp
            m_LerpData = new InterpolationData<float>();

            //Tick
            m_Tick.Initialize(tickDuration);

            //Arrows
            for (int i = 0; i < m_TickArrows.Length; i++)
                m_TickArrows[i].Initialize();

            InternalInitialize();
        }

        public void PerformUpdate(float deltaTime)
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


        public void PlayTickAnimation()
        {
            m_Tick.PlayTickAnimation();
        }

        public void PlayArrowsAnimation()
        {
            //Arrows
            for (int i = 0; i < m_TickArrows.Length; i++)
                m_TickArrows[i].PrepareForInterpolation();

            //Lerp
            m_LerpData.TotalTime = (float)RhytmController.GetInstance().TimeToNextTick * 2 + (float)RhytmController.GetInstance().ProcessTickDelta;
            m_LerpData.Start();
        }
    }
}
