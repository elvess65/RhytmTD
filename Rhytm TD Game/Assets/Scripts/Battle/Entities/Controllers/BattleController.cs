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
        private StartBattleSequenceModel m_StartBattleSequenceModel;


        public BattleController(Dispatcher dispatcher) : base(dispatcher)
        {
        }

        public override void InitializeComplete()
        {
            base.InitializeComplete();

            m_RhytmController = Dispatcher.GetController<RhytmController>();
            m_RhytmInputProxy = Dispatcher.GetController<RhytmInputProxy>();
            m_SpawnController = Dispatcher.GetController<SolidEntitySpawnController>();

            m_BattleModel = Dispatcher.GetModel<BattleModel>();
            m_BattleModel.OnBattleInitialize += Initialize;
            m_BattleModel.OnBattleStarted += BattleStartedHandler;
            m_BattleModel.OnBattleFinished += BattleFinishedHandler;
            m_BattleModel.OnSpellbookOpened += SpellBookOpenedHandler;
            m_BattleModel.OnSpellbookClosed += SpellBookClosedHandler;
            m_BattleModel.OnSpellbookPostUsed += SpellBookClosedHandler;

            m_UpdateModel = Dispatcher.GetModel<UpdateModel>();
            m_UpdateModel.OnUpdate += Update;

            m_SpawnModel = Dispatcher.GetModel<SpawnModel>();

            m_AudioModel = Dispatcher.GetModel<BattleAudioModel>();

            m_StartBattleSequenceModel = Dispatcher.GetModel<StartBattleSequenceModel>();
            m_StartBattleSequenceModel.OnSequenceFinished += StartLoop;
        }

        
        private void Initialize()
        {
            //Initialize StateMachine
            m_StateMachine = new BattleStateMachine<BattleState_Abstract>();
            m_StateMachine.AddState(new BattleState_LockInput());
            m_StateMachine.AddState(new BattleState_Normal());
            m_StateMachine.Initialize<BattleState_LockInput>();

            //Rhytm data
            m_AudioModel.BPM = 60;
            m_RhytmInputProxy.SetInputPrecious(0.25f);

            //Spawn player
            m_SpawnModel.OnShouldCreatePlayer?.Invoke();
        }

        private void Update(float deltaTime)
        {
            m_BattleModel.CheckEntitiesToDelete();
        }

      
        private void StartLoop()
        {
            m_RhytmController.StartTicking();
           
            m_BattleModel.OnBattleStarted?.Invoke();
        }

        private void BattleStartedHandler()
        {
            m_AudioModel.OnPlayMetronome(true);
            m_AudioModel.OnPlayMusic(true);

            m_StateMachine.ChangeState<BattleState_Normal>();
        }

        private void BattleFinishedHandler(bool isSuccess)
        {
            m_RhytmController.StopTicking();

            m_AudioModel.OnPlayMetronome(false);
            m_AudioModel.OnPlayMusic(false);

            m_StateMachine.ChangeState<BattleState_LockInput>();
        }

        private void SpellBookOpenedHandler()
        {
            m_StateMachine.ChangeState<BattleState_LockInput>();
        }

        private void SpellBookClosedHandler()
        {
            m_StateMachine.ChangeState<BattleState_Normal>();
        }
    }
}
