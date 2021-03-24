using CoreFramework;
using CoreFramework.Rhytm;
using RhytmTD.Battle.Entities.Models;
using RhytmTD.Battle.StateMachine;
using RhytmTD.Data.Models;

namespace RhytmTD.Battle.Entities.Controllers
{
    /// <summary>
    /// Контроллер боя
    /// </summary>
    public class BattleController : BaseController
    {
        private BattleStateMachine<BattleState_Abstract> m_StateMachine;

        private RhytmInputProxy m_RhytmInputProxy;
        private RhytmController m_RhytmController;
        private SolidEntitySpawnController m_SpawnController;

        private SpawnModel m_SpawnModel;
        private BattleModel m_BattleModel;
        private UpdateModel m_UpdateModel;
        private BattleAudioModel m_AudioModel;
        private SpellBookModel m_SpellBookModel;
        private StartBattleSequenceModel m_StartBattleSequenceModel;

        public BattleController(Dispatcher dispatcher) : base(dispatcher)
        {
        }

        public override void InitializeComplete()
        {
            base.InitializeComplete();

            m_SpawnModel = Dispatcher.GetModel<SpawnModel>();
            m_BattleModel = Dispatcher.GetModel<BattleModel>();
            m_UpdateModel = Dispatcher.GetModel<UpdateModel>();
            m_AudioModel = Dispatcher.GetModel<BattleAudioModel>();
            m_SpellBookModel = Dispatcher.GetModel<SpellBookModel>();
            m_StartBattleSequenceModel = Dispatcher.GetModel<StartBattleSequenceModel>();

            m_RhytmController = Dispatcher.GetController<RhytmController>();
            m_RhytmInputProxy = Dispatcher.GetController<RhytmInputProxy>();
            m_SpawnController = Dispatcher.GetController<SolidEntitySpawnController>();

            m_BattleModel.OnBattleInitialize += Initialize;

            m_SpellBookModel.OnSpellbookOpened += SpellBookOpenedHandler;
            m_SpellBookModel.OnDirectionalSpellSelected += SpellBookSelectedHandler;
            m_SpellBookModel.OnSpellbookUsed += SpellBookUsedHandler;
            m_SpellBookModel.OnSpellbookPostUsed += SpellBookClosedAndPostUsedHandler;
            m_SpellBookModel.OnSpellbookClosed += SpellBookClosedAndPostUsedHandler;

            m_UpdateModel.OnUpdate += Update;
            
            m_StartBattleSequenceModel.OnSequenceFinished += StartLoop;
        }
        
        private void Initialize()
        {
            //Initialize StateMachine
            m_StateMachine = new BattleStateMachine<BattleState_Abstract>();
            m_StateMachine.AddState(new BattleState_LockInput());
            m_StateMachine.AddState(new BattleState_Normal());
            m_StateMachine.AddState(new BattleState_Spellbook());
            m_StateMachine.AddState(new BattleState_SpellbookDirectionalSelected());
            m_StateMachine.Initialize<BattleState_LockInput>();

            //Rhytm data
            m_AudioModel.BPM = 60;
            m_RhytmInputProxy.SetInputPrecious(0.25f);

            //Spawn player
            m_SpawnModel.OnShouldCreatePlayer?.Invoke();
        }

        public void FinishBattle(bool isSuccess)
        {
            m_RhytmController.StopTicking();

            m_AudioModel.OnPlayMetronome(false);
            m_AudioModel.OnPlayMusic(false);

            TargetModule targetModule = m_BattleModel.PlayerEntity.GetModule<TargetModule>();
            targetModule.ClearTarget();

            m_StateMachine.ChangeState<BattleState_LockInput>();

            m_BattleModel.OnBattleFinished?.Invoke(isSuccess);
        }

        private void Update(float deltaTime)
        {
            m_BattleModel.CheckEntitiesToDelete();
        }
      
        private void StartLoop()
        {
            m_AudioModel.OnPlayMetronome(true);
            m_AudioModel.OnPlayMusic(true);

            m_StateMachine.ChangeState<BattleState_Normal>();

            m_RhytmController.StartTicking();
           
            m_BattleModel.OnBattleStarted?.Invoke();
        }

        private void SpellBookOpenedHandler()
        {
            m_StateMachine.ChangeState<BattleState_Spellbook>();
        }

        private void SpellBookSelectedHandler()
        {
            m_StateMachine.ChangeState<BattleState_SpellbookDirectionalSelected>();
        }

        private void SpellBookUsedHandler()
        {
            m_StateMachine.ChangeState<BattleState_LockInput>();
        }

        private void SpellBookClosedAndPostUsedHandler()
        {
            m_StateMachine.ChangeState<BattleState_Normal>();
        }
    }
}
