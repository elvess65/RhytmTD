using DG.Tweening;
using UnityEngine;

namespace RhytmTD.Animation.DOTween
{
    public abstract class TweenContainer
    {
        protected Transform m_ControlledTransofrm;

        public TweenContainer(Transform controlledTransform)
        {
            m_ControlledTransofrm = controlledTransform;
        }

        public abstract Tween GetTween();
    }
}
