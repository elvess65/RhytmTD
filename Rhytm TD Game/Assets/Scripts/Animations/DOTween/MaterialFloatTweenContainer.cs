using DG.Tweening;
using UnityEngine;

namespace RhytmTD.Animation.DOTween
{
    public class MaterialFloatTweenContainer : TweenContainer
    {
        [Header("Scale Tween")]
        [SerializeField] private string m_Property;
        [SerializeField] private float m_Value;

        public override Tween GetTween()
        {
            Material material = m_ControlledTransofrm.gameObject.GetComponent<MeshRenderer>().material;

            if (m_IsFrom)
                return material.DOFloat(m_Value, m_Property, m_Duration).SetDelay(m_Delay).From();

            return material.DOFloat(m_Value, m_Property, m_Duration).SetDelay(m_Delay);
        }
    }
}
