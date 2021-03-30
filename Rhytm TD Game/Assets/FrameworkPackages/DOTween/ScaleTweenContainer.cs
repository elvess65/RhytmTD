using DG.Tweening;
using UnityEngine;

namespace FrameworkPackages.DOTween
{
    public class ScaleTweenContainer : TweenContainer
    {
        [Header("Scale Tween")]
        [SerializeField] private Vector3 m_Scale = Vector3.zero;

        public override Tween GetTween()
        {
            if (m_IsFrom)
                return m_ControlledTransofrm.DOScale(m_Scale, m_Duration).SetDelay(m_Delay).From();

            return m_ControlledTransofrm.DOScale(m_Scale, m_Duration).SetDelay(m_Delay);
        }
    }
}
