using CoreFramework.Rhytm;
using RhytmTD.UI.View;
using RhytmTD.UI.Widget;
using UnityEngine;

namespace RhytmTD.UI.Battle.View
{
    /// <summary>
    /// Отображение виджетов боя в HUD
    /// </summary>
    public class UIView_BattleHUD : UIView_Abstract
    {
        [Space]
        public UIWidget_Tick UIWidget_Tick;

        public override void Initialize()
        {
            UIWidget_Tick.Initialize((float)RhytmController.GetInstance().TickDurationSeconds / 8);

            RegisterWidget(UIWidget_Tick);
        }
    }
}
