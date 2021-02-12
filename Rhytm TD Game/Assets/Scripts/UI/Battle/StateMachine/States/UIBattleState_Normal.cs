using CoreFramework.Rhytm;

namespace RhytmTD.UI.Battle.StateMachine
{
    public class UIBattleState_Normal : UIBattleState_Abstract
    {
        private RhytmController m_RhytmController;

        public UIBattleState_Normal() : base()
        {
            m_RhytmController = Dispatcher.GetController<RhytmController>();
        }

        public override void EnterState()
        {
            base.EnterState();

            //Events
            m_RhytmController.OnTick += TickHandler;
            m_RhytmController.OnEventProcessingTick += ProcessTickHandler;

            //UI
            m_UIModel.UIView_BattleHUD.SetWidgetsActive(true, true);

        }

        public override void ExitState()
        {
            base.ExitState();

            //Events
            m_RhytmController.OnTick -= TickHandler;
            m_RhytmController.OnEventProcessingTick -= ProcessTickHandler;
        }


        private void TickHandler(int ticksSinceStart)
        {
            //m_UIView_BattleHUD.UIWidget_Tick.PlayTickAnimation();
        }

        private void ProcessTickHandler(int ticksSinceStart)
        {
            //m_UIView_BattleHUD.UIWidget_Tick.PlayArrowsAnimation();
        }
    }
}
