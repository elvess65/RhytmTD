using DG.Tweening;
using UnityEngine;

namespace RhytmTD.Animation.DOTween
{
    public class ScaleTweenContainer : TweenContainer
    {
        private Vector3 m_Scale;
        private float m_Duration;
        private bool m_From;

        public ScaleTweenContainer(Transform controlledTransform, Vector3 scale, float duration, bool from) : base(controlledTransform)
        {
            m_Scale = scale;
            m_Duration = duration;
            m_From = from;
        }

        public override Tween GetTween()
        {
            if (m_From)
            {
                return m_ControlledTransofrm.DOScale(m_Scale, m_Duration).From();
            }

            return m_ControlledTransofrm.DOScale(m_Scale, m_Duration);
        }
    }
}
