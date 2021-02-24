using DG.Tweening;
using UnityEngine;

namespace RhytmTD.Animation.DOTween
{
    public class DOTweenSequenceAnimator : MonoBehaviour
    {
        public void PlaySequence(TweenContainer[] tweens)
        {
            Sequence sequence = DG.Tweening.DOTween.Sequence();

            for (int i = 0; i < tweens.Length; i++)
                sequence.Append(tweens[i].GetTween());
        }
    }
}
