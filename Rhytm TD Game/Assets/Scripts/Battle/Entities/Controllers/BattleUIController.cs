using CoreFramework;
using RhytmTD.Battle.Entities.Models;
using RhytmTD.UI.Battle.StateMachine;

namespace RhytmTD.Battle.Entities.Controllers
{
    /// <summary>
    /// Контроллер UI боя
    /// </summary>
    public class BattleUIController : BaseController
    {
        private UIBattleStateMachine<UIBattleState_Abstract> m_StateMachine;

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
            m_BattleModel.OnSpellbookOpened += SpellBookOpenedHandler;
            m_BattleModel.OnSpellbookClosed += SpellBookClosedHandler;
            m_BattleModel.OnSpellbookUsed += SpellBookUsedHandler;
            m_BattleModel.OnSpellbookPostUsed += SpellBookClosedHandler;
        }


        private void Initialize()
        {
            m_UIModel.UIView_BattleHUD.Initialize();
            m_UIModel.UIView_SpellbookHUD.Initialize();
            m_UIModel.UIView_BattleResultHUD.Initialize();

            m_StateMachine = new UIBattleStateMachine<UIBattleState_Abstract>();
            m_StateMachine.AddState(new UIBattleState_NoUI());
            m_StateMachine.AddState(new UIBattleState_Normal());
            m_StateMachine.AddState(new UIBattleState_BattleResult());
            m_StateMachine.AddState(new UIBattleState_Spellbook());
            m_StateMachine.AddState(new UIBattleState_SpellbookUsed());
            m_StateMachine.Initialize<UIBattleState_NoUI>();
        }

        private void BattleStartedHandler()
        {
            m_StateMachine.ChangeState<UIBattleState_Normal>();
        }

        private void BattleFinishedHandler(bool isSuccess)
        {
            m_StateMachine.ChangeState<UIBattleState_BattleResult>();
        }

        private void SpellBookOpenedHandler()
        {
            m_StateMachine.ChangeState<UIBattleState_Spellbook>();
        }

        private void SpellBookClosedHandler()
        {
            m_StateMachine.ChangeState<UIBattleState_Normal>();
        }

        private void SpellBookUsedHandler()
        {
            m_StateMachine.ChangeState<UIBattleState_SpellbookUsed>();
        }
    }
}
