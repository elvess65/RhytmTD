using CoreFramework.Rhytm;
using RhytmTD.UI.View;
using RhytmTD.UI.Widget;
using UnityEngine;

namespace RhytmTD.UI.Battle.View.UI
{
    /// <summary>
    /// Отображение виджетов боя в HUD
    /// </summary>
    public class UIView_BattleHUD : UIView_Abstract
    {
        private RhytmController m_RhytmController;

        [Space]
        public UIWidget_Tick UIWidget_Tick;

        public override void Initialize()
        {
            m_RhytmController = Dispatcher.GetController<RhytmController>();

            UIWidget_Tick.Initialize((float)m_RhytmController.TickDurationSeconds / 8);    
            RegisterWidget(UIWidget_Tick);
        }
    }
}
