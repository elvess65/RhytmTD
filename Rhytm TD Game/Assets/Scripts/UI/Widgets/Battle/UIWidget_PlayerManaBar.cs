using RhytmTD.Animation.DOTween;
using UnityEngine;
using UnityEngine.UI;

namespace RhytmTD.UI.Widget
{
    public class UIWidget_PlayerManaBar : UIWidget
    {
        [Space]

        [SerializeField] private Image Foreground = null;
        [SerializeField] private DOTweenSequenceAnimator AddSequenceAnimator = null;
        [SerializeField] private DOTweenSequenceAnimator RemoveSequenceAnimator = null;

        private RectTransform m_ForegroundRectTransform;
        private float m_FilledSize;

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

            m_ForegroundRectTransform.SetLeft(damageHaldOffset);
            m_ForegroundRectTransform.SetRight(damageHaldOffset);
        }

        public void PlayAddAnimation()
        {
            AddSequenceAnimator.PlaySequence();
        }

        public void PlayRemoveAnimation()
        {
            RemoveSequenceAnimator.PlaySequence();
        }
    }
}

