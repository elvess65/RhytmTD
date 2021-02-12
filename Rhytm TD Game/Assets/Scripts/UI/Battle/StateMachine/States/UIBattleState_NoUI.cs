using RhytmTD.UI.Battle.View;

namespace RhytmTD.UI.Battle.StateMachine
{
    public class UIBattleState_NoUI : UIBattleState_Abstract
    {
        private bool m_IsFirstEnter = true;

        public UIBattleState_NoUI(UIView_BattleHUD uiView_BattleHUD) : base(uiView_BattleHUD)
        {
        }

        public override void EnterState()
        {
            base.EnterState();

            //m_UIView_BattleHUD.SetWidgetsActive(false, !m_IsFirstEnter);
        }
    }
}
