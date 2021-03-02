namespace RhytmTD.UI.Battle.StateMachine
{
    public class UIBattleState_SpellbookUsed : UIBattleState_Abstract
    {
        public UIBattleState_SpellbookUsed() : base()
        {
        }

        public override void EnterState()
        {
            base.EnterState();

            //UI
            m_UIModel.UIView_BattleHUD.SetWidgetsActive(true, true);
            m_UIModel.UIView_BattleHUD.LockInput(true);
        }

        public override void ExitState()
        {
            base.ExitState();
        }
    }
}
