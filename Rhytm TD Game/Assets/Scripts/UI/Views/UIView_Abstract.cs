using System.Collections.Generic;
using CoreFramework;
using RhytmTD.UI.Widget;
using UnityEngine;

namespace RhytmTD.UI.View
{
    /// <summary>
    /// Базовый класс для UI View
    /// </summary>
    public abstract class UIView_Abstract : BaseView
    {
        public Transform Root;

        protected List<UIWidget> m_Widgets = new List<UIWidget>();


        public abstract void Initialize();

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
            {
                m_Widgets[i].SetWidgetActive(isEnabled, isAnimated);
            }
        }

        /// <summary>
        /// Регистрирует виджет с списке управляемых (для включения/отключения)
        /// </summary>
        /// <param name="widget"></param>
        protected void RegisterWidget(UIWidget widget)
        {
            m_Widgets.Add(widget);
        }
    }
}
