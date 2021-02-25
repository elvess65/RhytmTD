using DG.Tweening;
using UnityEngine;

namespace RhytmTD.Animation.DOTween
{
    public abstract class TweenContainer : MonoBehaviour
    {
        [Header("Base Tween")]
        [SerializeField] protected Transform m_ControlledTransofrm;
        [SerializeField] protected bool m_IsFrom;
        [SerializeField] protected float m_Delay;

        public abstract Tween GetTween();
    }
}
