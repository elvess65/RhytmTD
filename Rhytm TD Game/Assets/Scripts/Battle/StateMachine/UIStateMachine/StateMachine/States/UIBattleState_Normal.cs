namespace RhytmTD.UI.Battle.StateMachine
{
    public class UIBattleState_Normal : UIBattleState_Abstract
    {
        public UIBattleState_Normal() : base()
        {
        }

        public override void EnterState()
        {
            base.EnterState();

            //UI
            m_UIModel.UIView_BattleHUD.SetWidgetsActive(true, true);
            m_UIModel.UIView_BattleHUD.LockInput(false);
        }
    }
}
