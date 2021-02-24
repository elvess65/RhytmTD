using DG.Tweening;
using UnityEngine;

namespace RhytmTD.Animation.DOTween
{
    public class ShakeScaleTweenContainer : TweenContainer
    {
        private float m_Duration = 0.2f;
        private Vector3 m_Strength;
        private int m_Vibrato = 10;
        private int m_Randomness = 90;
        private bool m_ShakeFade = false;

        public ShakeScaleTweenContainer(Transform controlledTransform, float duration, Vector3 strength, int vibrato, int randomness, bool shakeFade) : base(controlledTransform)
        {
            m_Duration = duration;
            m_Strength = strength;
            m_Vibrato = vibrato;
            m_Randomness = randomness;
            m_ShakeFade = shakeFade;
        }

        public override Tween GetTween()
        {
            return m_ControlledTransofrm.DOShakeScale(m_Duration, m_Strength, m_Vibrato, m_Randomness, m_ShakeFade);
        }
    }
}

