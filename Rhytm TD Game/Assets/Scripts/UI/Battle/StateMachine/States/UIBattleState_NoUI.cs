namespace RhytmTD.UI.Battle.StateMachine
{
    public class UIBattleState_NoUI : UIBattleState_Abstract
    {
        private bool m_IsFirstEnter;

        public UIBattleState_NoUI() : base()
        {
            m_IsFirstEnter = true;
        }

        public override void EnterState()
        {
            base.EnterState();

            m_UIModel.UIView_BattleHUD.SetWidgetsActive(false, !m_IsFirstEnter);
            m_IsFirstEnter = false;
        }
    }
}
