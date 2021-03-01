using RhytmTD.UI.Widget;
using UnityEngine;

namespace RhytmTD.UI.View
{
    /// <summary>
    /// Отображение виджетов в главном меню
    /// </summary>
    public class UIView_MenuScene : UIView_Abstract
    {
        public System.Action OnPlayButtonPressHandler;

        [Space(10)]

        public UIWidget_Button UIWidget_ButtonPlay;
        public UIWidget_Currency UIWidget_Currency;

        public override void Initialize()
        {
            UIWidget_ButtonPlay.OnWidgetPress += ButtonPlay_Widget_PressHandler;
            UIWidget_ButtonPlay.Initialize();

            //UIWidget_Currency.Initialize(10);//GameManager.Instance.DataHolder.AccountModel.CurrencyAmount);

            RegisterWidget(UIWidget_ButtonPlay);
            //RegisterWidget(UIWidget_Currency);

            //RegisterUpdatable(UIWidget_Currency);
        }

        private void ButtonPlay_Widget_PressHandler()
        {
             LockInput(true);

             OnPlayButtonPressHandler?.Invoke();
        }
    }
}
