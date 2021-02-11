using System.Collections;
using CoreFramework;
using CoreFramework.Rhytm;
using RhytmTD.Battle.Entities.Models;
using RhytmTD.Battle.StateMachine;
using RhytmTD.Data.Controllers;
using UnityEngine;

namespace RhytmTD.Battle.Entities.Controllers
{
    /// <summary>
    /// Контроллер боя
    /// </summary>
    public class BattleController : BaseController
    {
        private RhytmInputProxy m_RhytmInputProxy;
        private RhytmController m_RhytmController;
        private SpawnController m_SpawnController;

        private BattleModel m_BattleModel;
        private BattleAudioModel m_AudioModel;

        public BattleStateMachine<BattleState_Abstract> StateMachine { get; private set; }


        public BattleController(Dispatcher dispatcher) : base(dispatcher)
        {
        }

        public override void InitializeComplete()
        {
            base.InitializeComplete();

            m_RhytmController = Dispatcher.GetController<RhytmController>();
            m_RhytmInputProxy = Dispatcher.GetController<RhytmInputProxy>();
            m_SpawnController = Dispatcher.GetController<SpawnController>();

            m_BattleModel = Dispatcher.GetModel<BattleModel>();
            m_BattleModel.OnBattleInitialize += Initialize;
            m_BattleModel.OnBattleStarted += BattleStartedHandler;
            m_BattleModel.OnBattleFinished += BattleFinishedHandler;

            m_AudioModel = Dispatcher.GetModel<BattleAudioModel>();
        }

        
        private void Initialize()
        {
            //Initialize StateMachine
            StateMachine = new BattleStateMachine<BattleState_Abstract>();
            StateMachine.AddState(new BattleState_LockInput());
            StateMachine.AddState(new BattleState_Normal());
            StateMachine.Initialize<BattleState_LockInput>();

            //Rhytm data
            m_AudioModel.BPM = 30;
            m_RhytmInputProxy.SetInputPrecious(0.25f);

            //Spawn player
            m_SpawnController.SpawnPlayer();

            Dispatcher.GetController<UpdateController>().UpdaterObject.GetComponent<MonoUpdater>().StartCoroutine(InitializationCoroutine());
        }

        private IEnumerator InitializationCoroutine()
        {
            yield return new WaitForSeconds(2);

            InitializationFinished();
        }

        private void InitializationFinished()
        {
            m_BattleModel.OnBattleStarted?.Invoke();
        }

        void BattleStartedHandler()
        {
            m_RhytmController.StartTicking();

            m_AudioModel.OnPlayMetronome(true);
            //m_AudioModel.OnPlayMusic(true);

            StateMachine.ChangeState<BattleState_Normal>();
        }

        void BattleFinishedHandler(bool isSuccess)
        {
            m_AudioModel.OnPlayMetronome(false);
            m_AudioModel.OnPlayMusic(false);

            StateMachine.ChangeState<BattleState_LockInput>();
        }
    }
}
