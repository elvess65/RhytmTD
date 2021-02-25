using DG.Tweening;
using UnityEngine;

namespace RhytmTD.Animation.DOTween
{
    public class DOTweenSequenceAnimator : MonoBehaviour
    {
        public TweenContainer[] ExposedTweens;

        public Sequence PlayExposedSequence(TweenCallback onComplete = null)
        {
            return PlaySequence(ExposedTweens, onComplete);
        }

        private Sequence PlaySequence(TweenContainer[] tweens, TweenCallback onComplete)
        {
            Sequence sequence = DG.Tweening.DOTween.Sequence();
            
            for (int i = 0; i < tweens.Length; i++)
                sequence.Append(tweens[i].GetTween());

            sequence.AppendCallback(onComplete);

            return sequence;
        }
    }
}
