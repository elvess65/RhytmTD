using System.Collections.Generic;
using CoreFramework;
using CoreFramework.Abstract;
using RhytmTD.UI.Widget;
using UnityEngine;

namespace RhytmTD.UI.View
{
    /// <summary>
    /// Базовый класс для UI View
    /// </summary>
    public abstract class UIView_Abstract : MonoBehaviour, iUpdatable
    {
        public Transform Root;

        protected List<UIWidget> m_Widgets = new List<UIWidget>();

        private UpdatablesManager m_UpdatablesManager = new UpdatablesManager();


        public abstract void Initialize();
 
        public virtual void PerformUpdate(float deltaTime)
        {
            m_UpdatablesManager?.PerformUpdate(deltaTime);
        }

        public virtual void LockInput(bool isLocked)
        { }

        /// <summary>
        /// Включить/выключить визуальное отображение виджета
        /// </summary>
        /// <param name="isEnabled">Включен/выключен виджет</param>
        /// <param name="isAnimated">Анимация при выполнении</param>
        public void SetWidgetsActive(bool isEnabled, bool isAnimated)
        {
            for (int i = 0; i < m_Widgets.Count; i++)
                m_Widgets[i].SetWidgetActive(isEnabled, isAnimated);
        }

        /// <summary>
        /// Регистрирует виджет с списке управляемых (для включения/отключения)
        /// </summary>
        /// <param name="widget"></param>
        protected void RegisterWidget(UIWidget widget) => m_Widgets.Add(widget);

        /// <summary>
        /// Регистрирует виджек как обновляемый (для Update)
        /// </summary>
        /// <param name="iUpdatable"></param>
        protected void RegisterUpdatable(iUpdatable iUpdatable) => m_UpdatablesManager.Add(iUpdatable);
    }
}
