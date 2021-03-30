using DG.Tweening;
using UnityEngine;

namespace FrameworkPackages.DOTween
{
    public class ShakeScaleTweenContainer : TweenContainer
    {
        [Header("Shake Scale Tween")]
        [SerializeField] private Vector3 m_Strength = Vector3.zero;
        [SerializeField] private int m_Vibrato = 10;
        [SerializeField] private int m_Randomness = 90;
        [SerializeField] private bool m_ShakeFade = false;

        public override Tween GetTween()
        {
            if (m_IsFrom)
                return m_ControlledTransofrm.DOShakeScale(m_Duration, m_Strength, m_Vibrato, m_Randomness, m_ShakeFade).SetDelay(m_Delay).From();

            return m_ControlledTransofrm.DOShakeScale(m_Duration, m_Strength, m_Vibrato, m_Randomness, m_ShakeFade).SetDelay(m_Delay);
        }
    }
}

