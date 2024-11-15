﻿using UnityEngine;
using UnityEngine.UI;

namespace RhytmTD.UI.Components
{
    /// <summary>
    /// Компонент объекта показывающего наступления тика
    /// </summary>
    public class UIComponent_WidgetMetronome_Tick : MonoBehaviour
    {
        [SerializeField] private Image m_ControlledImage = null;

        private WaitForSeconds m_WaitBeatIndicatorDelay;


        public void Initialize(float tickDuration)
        {
            m_WaitBeatIndicatorDelay = new WaitForSeconds(tickDuration);
        }

        public void StartPlayTickAnimation()
        {
            StartCoroutine(TickAnimationCoroutine());
        }

        private System.Collections.IEnumerator TickAnimationCoroutine()
        {
            m_ControlledImage.transform.localScale += new Vector3(0.1f, 0.1f, 0.1f);

            yield return m_WaitBeatIndicatorDelay;

            m_ControlledImage.transform.localScale -= new Vector3(0.1f, 0.1f, 0.1f);
        }
    }
}
