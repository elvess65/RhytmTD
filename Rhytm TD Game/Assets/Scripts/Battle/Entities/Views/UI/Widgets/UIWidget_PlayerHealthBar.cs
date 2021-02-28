using RhytmTD.Animation.DOTween;
using UnityEngine;
using UnityEngine.UI;

namespace RhytmTD.UI.Widget
{
    public class UIWidget_PlayerHealthBar : UIWidget
    {
        [Space]

        [SerializeField] private Image Foreground;
        [SerializeField] private DOTweenSequenceAnimator DamageSequenceAnimator;
        [SerializeField] private DOTweenSequenceAnimator HealSequenceAnimator;

        public void Initialize()
        {
            InternalInitialize();
        }

        public void UpdateHealthBar(int currentHeath, int health)
        {
            Foreground.fillAmount = currentHeath / (float)health;
        }

        public void PlayDamageAnimation()
        {
            DamageSequenceAnimator.PlaySequence();
        }

        public void PlayHealAnimation()
        {
            HealSequenceAnimator.PlaySequence();
        }
    }
}
