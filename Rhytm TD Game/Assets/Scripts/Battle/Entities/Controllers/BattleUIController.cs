using CoreFramework;
using RhytmTD.Battle.Entities.Models;
using RhytmTD.UI.Battle.StateMachine;
using RhytmTD.UI.Battle.View;
using UnityEngine;

namespace RhytmTD.Battle.Entities.Controllers
{
    /// <summary>
    /// Контроллер UI боя
    /// </summary>
    public class BattleUIController : BaseController
    {
        private UIBattleStateMachine<UIBattleState_Abstract> StateMachine;

        private BattleUIModel m_UIModel;
        private BattleModel m_BattleModel;

        public BattleUIController(Dispatcher dispatcher) : base(dispatcher)
        {
        }

        public override void InitializeComplete()
        {
            base.InitializeComplete();

            m_UIModel = Dispatcher.GetModel<BattleUIModel>();

            m_BattleModel = Dispatcher.GetModel<BattleModel>();
            m_BattleModel.OnBattleInitialize += Initialize;
            m_BattleModel.OnBattleStarted += BattleStartedHandler;
            m_BattleModel.OnBattleFinished += BattleFinishedHandler;
        }


        private void Initialize()
        {
            m_UIModel.UIView_BattleHUD.Initialize();

            StateMachine = new UIBattleStateMachine<UIBattleState_Abstract>();
            StateMachine.AddState(new UIBattleState_NoUI());
            StateMachine.AddState(new UIBattleState_Normal());
            StateMachine.Initialize<UIBattleState_NoUI>();
        }

        private void BattleStartedHandler()
        {
            StateMachine.ChangeState<UIBattleState_Normal>();
        }

        private void BattleFinishedHandler(bool isSuccess)
        {
            StateMachine.ChangeState<UIBattleState_NoUI>();
        }
    }
}
