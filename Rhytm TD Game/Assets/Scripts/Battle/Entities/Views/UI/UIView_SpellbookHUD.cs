using CoreFramework.Rhytm;
using RhytmTD.Battle.Entities.Models;
using RhytmTD.UI.View;
using RhytmTD.UI.Widget;
using UnityEngine;

namespace RhytmTD.UI.Battle.View.UI
{
    /// <summary>
    /// Отображение виджетов боя в HUD
    /// </summary>
    public class UIView_SpellbookHUD : UIView_Abstract
    {
        private BattleModel m_BattleModel;
        private RhytmController m_RhytmController;

        [Space]
        [SerializeField] private UIWidget_Button UIWidget_ButtonClose;

        public override void Initialize()
        {
            m_BattleModel = Dispatcher.GetModel<BattleModel>();
            m_RhytmController = Dispatcher.GetController<RhytmController>();

            UIWidget_ButtonClose.Initialize();
            UIWidget_ButtonClose.OnWidgetPress += ButtonCloseWidgetPressHandler;
            RegisterWidget(UIWidget_ButtonClose);
        }

        private void ButtonCloseWidgetPressHandler()
        {
            m_BattleModel.OnSpellbookExit?.Invoke();
        }
    }
}
