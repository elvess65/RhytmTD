using DG.Tweening;
using UnityEngine;

namespace FrameworkPackages.DOTween
{
    public abstract class TweenContainer : MonoBehaviour
    {
        [Header("Base Tween")]
        [SerializeField] protected Ease m_Ease = Ease.Linear;
        [SerializeField] protected Transform m_ControlledTransofrm;
        [SerializeField] protected bool m_IsFrom;
        [SerializeField] protected float m_Delay;
        [SerializeField] protected float m_Duration;

        public abstract Tween GetTween();

        public virtual void PrewarmTween()
        { }
    }
}
