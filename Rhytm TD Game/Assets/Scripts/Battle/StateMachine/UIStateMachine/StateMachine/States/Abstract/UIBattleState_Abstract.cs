using CoreFramework.StateMachine;
using RhytmTD.Battle.Entities.Models;

namespace RhytmTD.UI.Battle.StateMachine
{
    public abstract class UIBattleState_Abstract : AbstractState
    {
        protected BattleUIModel m_UIModel;

        public UIBattleState_Abstract()
        {
            m_UIModel = Dispatcher.GetModel<BattleUIModel>();
        }
    }
}
