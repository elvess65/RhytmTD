using UnityEngine;

namespace RhytmTD.UI.Widget
{
    public class UIWidget_PlayerHealthBar : UIWidget_CenteredBar
    {
        [Header("Visual Change")]
        [SerializeField] private Sprite m_NormalSprite = null;
        [SerializeField] private Sprite m_WarningSprite = null;
        [SerializeField] private Sprite m_DangerSprite = null;

        private const float m_WARNING_BOUNDS = 0.4f;
        private const float m_DANGER_BOUNDS = 0.25f;


        protected override void HandleVisualChange(float progress)
        {
            if (progress <= m_DANGER_BOUNDS)
                m_UIComponent_CenteredBar.ExposedForeground.sprite = m_DangerSprite;
            else if (progress < m_WARNING_BOUNDS)
                m_UIComponent_CenteredBar.ExposedForeground.sprite = m_WarningSprite;
            else
                m_UIComponent_CenteredBar.ExposedForeground.sprite = m_NormalSprite;
        }
    }
}
