﻿using DG.Tweening;
using UnityEngine;

namespace RhytmTD.Animation.DOTween
{
    public class MaterialFloatTweenContainer : TweenContainer
    {
        [Header("Scale Tween")]
        [SerializeField] private string m_Property;
        [SerializeField] private float m_Value;

        private Material m_Taterial;

        public override void PrewarmTween()
        {
            base.PrewarmTween();

            m_Taterial = m_ControlledTransofrm.gameObject.GetComponent<MeshRenderer>().material;
        }

        public override Tween GetTween()
        {
            if (m_IsFrom)
                return m_Taterial.DOFloat(m_Value, m_Property, m_Duration).SetDelay(m_Delay).From().SetEase(m_Ease);

            return m_Taterial.DOFloat(m_Value, m_Property, m_Duration).SetDelay(m_Delay).SetEase(m_Ease);
        }
    }
}
