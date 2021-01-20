using CoreFramework.StateMachine;
using UnityEngine;

namespace RhytmTD.Battle.StateMachine
{
    public class BattleStateMachine<T> : AbstractStateMachine<T> where T: BattleState_Abstract
    {
        public void HandleTouch(Vector3 mouseScreenPos)
        {
            m_CurrentState.HandleTouch(mouseScreenPos);
        }
    }
}
