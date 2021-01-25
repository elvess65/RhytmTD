using CoreFramework.Rhytm;
using UnityEngine;

namespace RhytmTD.Battle.StateMachine
{
    public class BattleState_Normal : BattleState_Abstract
    {
        public BattleState_Normal(RhytmInputProxy rhytmInputProxy) : base(rhytmInputProxy)
        {
        }

        public override void HandleTouch(Vector3 mouseScreenPos)
        {
            if (m_RhytmInputProxy.IsInputAllowed() && m_RhytmInputProxy.IsInputTickValid())
                Debug.Log("Input is valid");

            base.HandleTouch(mouseScreenPos);
        }       
    }
}
