using CoreFramework.Utils;
using UnityEngine;

namespace RhytmTD.UI.Components
{
    /// <summary>
    /// Контролирует группу интерполируемых элементов 
    /// </summary>
    public class UIComponent_InterpolatableGroup : MonoBehaviour
    {
        [SerializeField] private InterpolatableComponent[] m_ControlledObjects = null;

        private InterpolationData<float> m_LerpData;

        public bool IsInProgress => m_LerpData.IsStarted;


        public void Initialize(float duration)
        {
            m_LerpData = new InterpolationData<float>(duration);
            m_LerpData.From = 0;
            m_LerpData.To = m_LerpData.TotalTime;

            for (int i = 0; i < m_ControlledObjects.Length; i++)
                m_ControlledObjects[i].Initialize();
        }

        public void Execute()
        {
            for (int i = 0; i < m_ControlledObjects.Length; i++)
                m_ControlledObjects[i].PrepareForInterpolation();

            m_LerpData.Start();
        }

        public void Update()
        {
            if (m_LerpData.IsStarted)
            {
                m_LerpData.Increment();
                for (int i = 0; i < m_ControlledObjects.Length; i++)
                    m_ControlledObjects[i].ProcessInterpolation(m_LerpData.Progress);

                if (m_LerpData.Overtime())
                {
                    m_LerpData.Stop();

                    for (int i = 0; i < m_ControlledObjects.Length; i++)
                        m_ControlledObjects[i].FinishInterpolation();
                }
            }
        }
    }
}
