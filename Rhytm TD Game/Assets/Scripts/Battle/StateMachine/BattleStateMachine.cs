using CoreFramework.StateMachine;
using UnityEngine;

namespace RhytmTD.Battle.StateMachine
{
    public class BattleStateMachine : AbstractStateMachine
    {
        private BattleState_Abstract m_CurrentGameState;


        public void HandleTouch(Vector3 mouseScreenPos)
        {
            m_CurrentGameState.HandleTouch(mouseScreenPos);
        }


        protected override void SetState(AbstractState state)
        {
            base.SetState(state);

            m_CurrentGameState = m_CurrentState as BattleState_Abstract;
        }
    }
}
