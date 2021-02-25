using DG.Tweening;
using UnityEngine;

namespace RhytmTD.Animation.DOTween
{
    public class ScaleTweenContainer : TweenContainer
    {
        [Header("Scale Tween")]
        [SerializeField] private Vector3 m_Scale;
        [SerializeField] private float m_Duration;

        public override Tween GetTween()
        {
            if (m_IsFrom)
            {
                return m_ControlledTransofrm.DOScale(m_Scale, m_Duration).From();
            }

            return m_ControlledTransofrm.DOScale(m_Scale, m_Duration);
        }
    }
}
