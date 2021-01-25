using CoreFramework.Rhytm;
using UnityEngine;

namespace RhytmTD.Battle.StateMachine
{
    public class BattleState_LockInput : BattleState_Abstract
    {
        public BattleState_LockInput(RhytmInputProxy rhytmInputProxy) : base(rhytmInputProxy)
        {
        }

        public override void EnterState()
        {
            base.EnterState();

            //UI
            //UIView_PlayerHUD.SetWidgetsActive(false, IsUIHidedWithAnimation());
        }

        public override void HandleTouch(Vector3 mouseScreenPos)
        {
        }
    }
}
