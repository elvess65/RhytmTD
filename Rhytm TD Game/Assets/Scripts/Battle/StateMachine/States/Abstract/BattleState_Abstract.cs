using CoreFramework.Rhytm;
using CoreFramework.StateMachine;
using UnityEngine;

namespace RhytmTD.Battle.StateMachine
{
    public abstract class BattleState_Abstract : AbstractState
    {
        protected RhytmInputProxy m_RhytmInputProxy;

        public BattleState_Abstract(RhytmInputProxy rhytmInputProxy)
        {
            m_RhytmInputProxy = rhytmInputProxy;
        }

        public virtual void HandleTouch(Vector3 mouseScreenPos)
        {
            m_RhytmInputProxy.RegisterInput();
        }

        public override void PerformUpdate(float deltaTime)
        {

        }
    }
}
