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
        [SerializeField] private UIWidget_Tick m_UIWidget_Tick;

        public override void Initialize()
        {
            m_UIWidget_Tick.Initialize((float)RhytmController.GetInstance().TickDurationSeconds / 8,
                                       (float)RhytmController.GetInstance().TimeToNextTick + (float)RhytmController.GetInstance().ProcessTickDelta);

            RegisterWidget(m_UIWidget_Tick);
            RegisterUpdatable(m_UIWidget_Tick);
        }

        public override void PerformUpdate(float deltaTime)
        {
            base.PerformUpdate(deltaTime);

            m_UIWidget_Tick.PerformUpdate(deltaTime);
        }
    }
}
