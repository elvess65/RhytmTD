using RhytmTD.Animation.DOTween;
using UnityEngine;
using UnityEngine.UI;

namespace RhytmTD.UI.Widget
{
    public class UIWidget_PlayerHealthBar : UIWidget
    {
        [Space]

        [SerializeField] private Image Foreground = null;
        [SerializeField] private DOTweenSequenceAnimator DamageSequenceAnimator = null;
        [SerializeField] private DOTweenSequenceAnimator HealSequenceAnimator = null;

        [Header("Visual Change")]
        [SerializeField] private Sprite m_NormalSprite;
        [SerializeField] private Sprite m_WarningSprite;
        [SerializeField] private Sprite m_DangerSprite;

        private RectTransform m_ForegroundRectTransform;
        private float m_FilledSize;

        private const float m_WARNING_BOUNDS = 0.4f;
        private const float m_DANGER_BOUNDS = 0.25f;


        public void Initialize()
        {
            m_ForegroundRectTransform = Foreground.rectTransform;
            m_FilledSize = Root.GetComponent<RectTransform>().sizeDelta.x;

            InternalInitialize();
        }

        public void UpdateHealthBar(int currentHeath, int health)
        {
            float progress = currentHeath / (float)health;
            float damageOffset = m_FilledSize - progress * m_FilledSize;
            float damageHaldOffset = damageOffset / 2;

            HandleVisualChange(progress);

            m_ForegroundRectTransform.SetLeft(damageHaldOffset);
            m_ForegroundRectTransform.SetRight(damageHaldOffset);
        }

        public void PlayDamageAnimation()
        {
            DamageSequenceAnimator.PlaySequence();
        }

        public void PlayHealAnimation()
        {
            HealSequenceAnimator.PlaySequence();
        }


        private void HandleVisualChange(float progress)
        {
            if (progress <= m_DANGER_BOUNDS)
                Foreground.sprite = m_DangerSprite;
            else if (progress < m_WARNING_BOUNDS)
                Foreground.sprite = m_WarningSprite;
            else 
                Foreground.sprite = m_NormalSprite;
        }
    }
}
