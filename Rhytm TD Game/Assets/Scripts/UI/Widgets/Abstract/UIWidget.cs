using CoreFramework;
using RhytmTD.Data.Models;
using System;
using UnityEngine;

namespace RhytmTD.UI.Widget
{
    /// <summary>
    /// Базовый класс виджета
    /// </summary>
    public abstract class UIWidget : BaseView, IDisposable
    {
        public Transform Root;

        protected UpdateModel m_UpdateModel;

        private bool m_IsEnabled = true;
        private bool m_IsActive = false;
        private float m_TotalTime = 1;
        private float m_CurrentTime;
        private Vector2 m_FromToData = Vector2.zero;

        public void SetWidgetActive(bool isEnabled, bool isAnimated)
        {
            if (m_IsEnabled == isEnabled)
                return;

            m_IsEnabled = isEnabled;

            if (isAnimated)
            {
                m_FromToData.x = Root.localScale.x;
                m_FromToData.y = isEnabled ? 1 : 0;
                m_CurrentTime = 0;
                m_IsActive = true;
            }
            else 
                Root.localScale = isEnabled ? Vector3.one : Vector3.zero;
        }

        public virtual void LockInput(bool isLocked)
        { }

        protected virtual void InternalInitialize()
        {
            Dispatcher.AddDisposable(this);

            m_UpdateModel = Dispatcher.GetModel<UpdateModel>();
            m_UpdateModel.OnUpdate += WidgetUpdate;
        }

        protected virtual void WidgetUpdate(float deltaTime)
        {
            PlayActivationAnimation();
        }

        private void PlayActivationAnimation()
        {
            if (m_IsActive)
            {
                m_CurrentTime += Time.deltaTime;

                float size = Mathf.Lerp(m_FromToData.x, m_FromToData.y, m_CurrentTime / m_TotalTime);
                Root.localScale = new Vector3(size, size, Root.localScale.z);

                if (m_CurrentTime >= m_TotalTime)
                {
                    m_IsActive = false;
                    Root.localScale = new Vector3(m_FromToData.y, m_FromToData.y, Root.localScale.z);
                }
            }
        }

        public virtual void Dispose()
        {
            if (m_UpdateModel != null)
                m_UpdateModel.OnUpdate -= WidgetUpdate;

            Dispatcher.RemoveDisposable(this);
        }
    }
}
