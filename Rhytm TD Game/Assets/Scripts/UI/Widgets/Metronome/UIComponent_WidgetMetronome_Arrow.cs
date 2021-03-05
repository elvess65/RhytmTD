using CoreFramework.Rhytm;
using UnityEngine;
using UnityEngine.UI;

namespace RhytmTD.UI.Components
{
    /// <summary>
    /// Компонент стрелки показывающей момент наступления следующего тика
    /// </summary>
    public class UIComponent_WidgetMetronome_Arrow : InterpolatableComponent
    {
        [SerializeField] private RectTransform m_ControlledTransform = null;
        [SerializeField] private Image m_ArrowImage = null;

        private Vector3 m_InitPos;
        private Vector3 m_InitScale;

        private Color m_ColorOutOfRange;
        private Color m_ColorInRange;

        private RhytmInputProxy m_RhytmInputProxy;


        public void InitializeData(Color colorOutOfRange, Color colorInRange, RhytmInputProxy rhytmInputProxy)
        {
            m_ColorOutOfRange = colorOutOfRange;
            m_ColorInRange = colorInRange;

            m_RhytmInputProxy = rhytmInputProxy;
        }

        public override void Initialize()
        {
            m_InitPos = m_ControlledTransform.anchoredPosition;
            m_InitScale = m_ControlledTransform.localScale;
        }

        public override void PrepareForInterpolation()
        {
            m_ControlledTransform.gameObject.SetActive(true);
        }

        public override void FinishInterpolation()
        {
            m_ControlledTransform.gameObject.SetActive(false);
        }

        public override void ProcessInterpolation(float progress)
        {
            m_ControlledTransform.anchoredPosition = Vector3.Lerp(m_InitPos, Vector3.zero, progress);
            m_ControlledTransform.localScale = m_InitScale * Mathf.Lerp(0, 1, progress);

            m_ArrowImage.color = m_RhytmInputProxy.IsInUseRange ? m_ColorInRange : m_ColorOutOfRange;
        }
    }
}
