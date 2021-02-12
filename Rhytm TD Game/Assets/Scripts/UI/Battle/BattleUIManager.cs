using CoreFramework;
using RhytmTD.Battle.Entities.Models;
using RhytmTD.UI.Battle.StateMachine;
using RhytmTD.UI.Battle.View;
using UnityEngine;

namespace RhytmTD.UI.Battle
{
    public class BattleUIManager : MonoBehaviour
    {
        [SerializeField] private UIView_BattleHUD m_UIView_BattleHUD;

        private UIBattleStateMachine<UIBattleState_Abstract> m_StateMachine;

        private void Start()
        {
            Initialize();
        }

        public void Initialize()
        {
            InitializeViews();
            InitializeStateMachine();
        }

        private void InitializeViews()
        {
            m_UIView_BattleHUD.Initialize();
        }

        private void InitializeStateMachine()
        {
            Dispatcher.Instance.GetModel<BattleModel>().OnBattleStarted += BattleStarted;

            m_StateMachine = new UIBattleStateMachine<UIBattleState_Abstract>();
            m_StateMachine.AddState(new UIBattleState_NoUI(m_UIView_BattleHUD));
            m_StateMachine.AddState(new UIBattleState_Normal(m_UIView_BattleHUD));
            m_StateMachine.Initialize<UIBattleState_NoUI>();
        }

        void BattleStarted()
        {
            m_StateMachine.ChangeState<UIBattleState_Normal>();
        }

        public void ChangeState<T>() where T: UIBattleState_Abstract
        {
            m_StateMachine.ChangeState<T>();
        }
    }
}
