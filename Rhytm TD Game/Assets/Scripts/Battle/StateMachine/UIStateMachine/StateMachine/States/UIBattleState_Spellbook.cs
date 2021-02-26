namespace RhytmTD.UI.Battle.StateMachine
{
    public class UIBattleState_Spellbook : UIBattleState_Abstract
    {
        public UIBattleState_Spellbook() : base()
        {
        }

        public override void EnterState()
        {
            base.EnterState();

            //UI
            m_UIModel.UIView_BattleHUD.SetWidgetsActive(false, true);
            m_UIModel.UIView_SpellbookHUD.SetWidgetsActive(true, true);
        }

        public override void ExitState()
        {
            base.ExitState();

            //UI
            m_UIModel.UIView_SpellbookHUD.SetWidgetsActive(false, true);
        }
    }
}
