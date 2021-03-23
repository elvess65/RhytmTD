using RhytmTD.Animation.DOTween;
using RhytmTD.UI.Components;
using UnityEngine;

namespace RhytmTD.UI.Widget
{
    public abstract class UIWidget_CenteredBar : UIWidget
    {
        [Space]

        [SerializeField] protected UIComponent_CenteredBar m_UIComponent_CenteredBar;
        [SerializeField] protected DOTweenSequenceAnimator m_AddSequenceAnimator = null;
        [SerializeField] protected DOTweenSequenceAnimator m_RemoveSequenceAnimator = null;


        public virtual void Initialize()
        {
            m_UIComponent_CenteredBar.Initialize(Root.GetComponent<RectTransform>());

            InternalInitialize();
        }

        public void UpdateBar(int currentValue, int maxValue)
        {
            float progress = currentValue / (float)maxValue;
            m_UIComponent_CenteredBar.UpdateBar(progress);

            HandleVisualChange(progress);
        }

        public void PlayAddAnimation()
        {
            m_AddSequenceAnimator?.PlaySequence();
        }

        public void PlayRemoveAnimation()
        {
            m_RemoveSequenceAnimator?.PlaySequence();
        }


        protected abstract void HandleVisualChange(float progress);
    }
}
