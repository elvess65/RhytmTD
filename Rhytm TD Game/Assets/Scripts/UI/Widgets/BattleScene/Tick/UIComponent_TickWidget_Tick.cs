using CoreFramework;
using RhytmTD.Data.Controllers;
using UnityEngine;
using UnityEngine.UI;

namespace RhytmTD.UI.Components
{
    /// <summary>
    /// Компонент наступления тика
    /// </summary>
    public class UIComponent_TickWidget_Tick : MonoBehaviour
    {
        [SerializeField] private Image m_ControlledImage;

        private WaitForSeconds m_WaitBeatIndicatorDelay;


        public void Initialize(float tickDuration)
        {
            m_WaitBeatIndicatorDelay = new WaitForSeconds(tickDuration);
        }

        public void PlayTickAnimation()
        {
            Dispatcher.Instance.CreateController<UpdateController>().UpdaterObject.GetComponent<MonoUpdater>().StartCoroutine(TickAnimationCoroutine());
        }


        private System.Collections.IEnumerator TickAnimationCoroutine()
        {
            m_ControlledImage.transform.localScale += new Vector3(0.1f, 0.1f, 0.1f);

            yield return m_WaitBeatIndicatorDelay;

            m_ControlledImage.transform.localScale -= new Vector3(0.1f, 0.1f, 0.1f);
        }
    }
}
