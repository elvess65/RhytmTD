namespace RhytmTD.UI.Battle.StateMachine
{
    public class UIBattleState_BattleResult : UIBattleState_Abstract
    {
        public UIBattleState_BattleResult() : base()
        {
        }

        public override void EnterState()
        {
            base.EnterState();

            //UI
            m_UIModel.UIView_BattleHUD.SetWidgetsActive(false, true);
            m_UIModel.UIView_BattleResultHUD.SetWidgetsActive(true, true);
        }
    }
}
