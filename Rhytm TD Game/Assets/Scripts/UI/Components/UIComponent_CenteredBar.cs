using UnityEngine;
using UnityEngine.UI;

namespace RhytmTD.UI.Components
{
    /// <summary>
    /// Отцентрированный посередине бар
    /// </summary>
    public class UIComponent_CenteredBar : MonoBehaviour
    {
        [SerializeField] protected Image m_Foreground = null;

        private RectTransform m_ForegroundRectTransform;
        private float m_FilledSize;

        public Image ExposedForeground => m_Foreground;


        public void Initialize(RectTransform root)
        {
            m_ForegroundRectTransform = m_Foreground.rectTransform;
            m_FilledSize = root.sizeDelta.x;
        }

        public void UpdateBar(float progress)
        {
            float offset = m_FilledSize - progress * m_FilledSize;
            float halfOffset = offset / 2;

            m_ForegroundRectTransform.SetLeft(halfOffset);
            m_ForegroundRectTransform.SetRight(halfOffset);
        }
    }
}
