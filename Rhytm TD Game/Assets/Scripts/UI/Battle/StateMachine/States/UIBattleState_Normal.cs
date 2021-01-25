using CoreFramework.Rhytm;
using RhytmTD.UI.Battle.View;

namespace RhytmTD.UI.Battle.StateMachine
{
    public class UIBattleState_Normal : UIBattleState_Abstract
    {
        public UIBattleState_Normal(UIView_BattleHUD uiView_BattleHUD) : base(uiView_BattleHUD)
        {
        }

        public override void EnterState()
        {
            base.EnterState();

            //Events
            RhytmController.GetInstance().OnTick += TickHandler;
            RhytmController.GetInstance().OnEventProcessingTick += ProcessTickHandler;

            //UI
            m_UIView_BattleHUD.SetWidgetsActive(true, true);

        }

        public override void ExitState()
        {
            base.ExitState();

            //Events
            RhytmController.GetInstance().OnTick -= TickHandler;
            RhytmController.GetInstance().OnEventProcessingTick -= ProcessTickHandler;
        }


        private void TickHandler(int ticksSinceStart)
        {
            m_UIView_BattleHUD.UIWidget_Tick.PlayTickAnimation();
        }

        private void ProcessTickHandler(int ticksSinceStart)
        {
            UnityEngine.Debug.Log(ticksSinceStart);
            m_UIView_BattleHUD.UIWidget_Tick.PlayArrowsAnimation();
        }
    }
}
