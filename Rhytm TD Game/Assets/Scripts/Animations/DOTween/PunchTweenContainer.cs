using DG.Tweening;
using UnityEngine;

namespace RhytmTD.Animation.DOTween
{
    public class PunchTweenContainer : TweenContainer
    {
        [Header("Punch Tween")]
        [SerializeField] private Vector3 m_Punch = new Vector3(0.5f, 0, 0);
        [SerializeField] private int m_Vibrato = 1;
        [SerializeField] private int m_Elasticity = 1;

        public override Tween GetTween()
        {
            if (m_IsFrom)
                return m_ControlledTransofrm.DOPunchScale(m_Punch, m_Duration, m_Vibrato, m_Elasticity).SetDelay(m_Delay).From();

            return m_ControlledTransofrm.DOPunchScale(m_Punch, m_Duration, m_Vibrato, m_Elasticity).SetDelay(m_Delay);
        }
    }
}
