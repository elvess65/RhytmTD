using CoreFramework;
using CoreFramework.Abstract;
using RhytmTD.UI.Battle.StateMachine;
using RhytmTD.UI.Battle.View;
using UnityEngine;

namespace RhytmTD.UI.Battle
{
    public class BattleUIManager : MonoBehaviour, iUpdatable
    {
        public UIView_BattleHUD UIView_BattleHUD;

        private UIBattleStateMachine<UIBattleState_Abstract> m_StateMachine;
        private UpdatablesManager m_UpdatablesManager;


        public void Initialize()
        {
            InitializeViews();
            InitializeStateMachine();
            InitializeUpdatables();
        }

        public void PerformUpdate(float deltaTime)
        {
            m_UpdatablesManager?.PerformUpdate(deltaTime);
        }


        private void InitializeViews()
        {
            UIView_BattleHUD.Initialize();
        }

        private void InitializeStateMachine()
        {
            m_StateMachine = new UIBattleStateMachine<UIBattleState_Abstract>();
            m_StateMachine.AddState(new UIBattleState_NoUI());
            m_StateMachine.Initialize<UIBattleState_NoUI>();
        }

        private void InitializeUpdatables()
        {
            m_UpdatablesManager = new UpdatablesManager();
            m_UpdatablesManager.Add(UIView_BattleHUD);
        }
    }
}
