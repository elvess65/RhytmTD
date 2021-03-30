using DG.Tweening;
using UnityEngine;

namespace FrameworkPackages.DOTween
{
    public class DOTweenSequenceAnimator : MonoBehaviour
    {
        public TweenContainer[] ExposedTweens;

        private bool m_IsPlaying = false;

        public void PrewarmSequence()
        {
            for (int i = 0; i < ExposedTweens.Length; i++)
                ExposedTweens[i].PrewarmTween();
        }

        public Sequence PlaySequence(TweenCallback onCompleteCallback = null)
        {
            return PlaySequence(ExposedTweens, onCompleteCallback);
        }

        private Sequence PlaySequence(TweenContainer[] tweens, TweenCallback onComplete)
        {
            if (m_IsPlaying)
                return null;

            m_IsPlaying = true;

            Sequence sequence = DG.Tweening.DOTween.Sequence();
            
            for (int i = 0; i < tweens.Length; i++)
                sequence.Append(tweens[i].GetTween());

            sequence.AppendCallback(InternalCallBack);
            sequence.AppendCallback(onComplete);

            return sequence;
        }

        private void InternalCallBack()
        {
            m_IsPlaying = false;
        }
    }
}
