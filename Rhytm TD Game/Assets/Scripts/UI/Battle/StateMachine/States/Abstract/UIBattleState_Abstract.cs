using CoreFramework.StateMachine;
using RhytmTD.UI.Battle.View;

namespace RhytmTD.UI.Battle.StateMachine
{
    public abstract class UIBattleState_Abstract : AbstractState
    {
        protected UIView_BattleHUD m_UIView_BattleHUD;

        public UIBattleState_Abstract(UIView_BattleHUD uiView_BattleHUD)
        {
            m_UIView_BattleHUD = uiView_BattleHUD;
        }

        public override void Update(float deltaTime)
        {

        }
    }
}
