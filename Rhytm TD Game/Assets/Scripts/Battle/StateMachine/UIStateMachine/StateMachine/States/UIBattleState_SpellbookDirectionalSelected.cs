namespace RhytmTD.UI.Battle.StateMachine
{
    public class UIBattleState_SpellbookDirectionalSelected : UIBattleState_Abstract
    {
        public UIBattleState_SpellbookDirectionalSelected() : base()
        {
        }

        public override void EnterState()
        {
            base.EnterState();

            m_UIModel.UIView_SpellbookHUD.ExposedUIWidget_SkillDirectionSelection.SetWidgetActive(true, true);
        }

        public override void ExitState()
        {
            base.ExitState();

            m_UIModel.UIView_SpellbookHUD.ExposedUIWidget_SkillDirectionSelection.SetWidgetActive(false, true);
        }
    }
}
