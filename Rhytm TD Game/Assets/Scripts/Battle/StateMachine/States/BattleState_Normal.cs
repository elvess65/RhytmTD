using CoreFramework.Rhytm;
using UnityEngine;

namespace RhytmTD.Battle.StateMachine
{
    public class BattleState_Normal : BattleState_Abstract
    {
        public BattleState_Normal(RhytmInputProxy rhytmInputProxy) : base(rhytmInputProxy)
        {
        }

        public override void EnterState()
        {
            base.EnterState();

            //Events
            //Rhytm.RhytmController.GetInstance().OnTick += TickHandler;

            //UI
            //UIView_PlayerHUD.SetWidgetsActive(true, true);

        }

        public override void ExitState()
        {
            base.ExitState();

            //Events
            //Rhytm.RhytmController.GetInstance().OnTick -= TickHandler;
        }

        public override void HandleTouch(Vector3 mouseScreenPos)
        {
            Debug.Log(m_RhytmInputProxy.IsInputAllowed() + " " + m_RhytmInputProxy.IsInputTickValid());

            base.HandleTouch(mouseScreenPos);
        }


        private void TickHandler(int ticksSinceStart)
        {
            //UIView_PlayerHUD.UIWidget_Tick.PlayTickAnimation();
        }
    }
}
