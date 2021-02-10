using CoreFramework;
using CoreFramework.Abstract;
using CoreFramework.Input;
using CoreFramework.Rhytm;
using CoreFramework.Utils;
using RhytmTD.Battle.Entities;
using RhytmTD.Battle.Entities.Controllers;
using RhytmTD.Battle.Entities.Models;
using RhytmTD.Battle.Spawn;
using RhytmTD.Battle.StateMachine;
using RhytmTD.Data.Models;
using RhytmTD.Data.Models.DataTableModels;
using RhytmTD.UI.Battle.StateMachine;
using UnityEngine;

namespace RhytmTD.Battle.Core
{
    public class BattleManager : Singleton<BattleManager>
    {
        public MonoReferencesHolder MonoReferencesHolder;

        [Header("Temp")]
        public Metronome Metronome;
        public AudioSource Music;

        private BattleStateMachine<BattleState_Abstract> m_StateMachine;

        private Dispatcher m_Dispatcher;
        private RhytmInputProxy m_RhytmInputProxy;
        private RhytmController m_RhytmController;
        private InputController m_InputController;
        private SpawnController m_SpawnController;
        private DamageController m_DamageController;
        private BattlefieldController m_BattlefieldController;

        private BattleModel m_BattleModel;
        private WorldDataModel m_WorldDataModel;
        private AccountDataModel m_AccountDataModel;

        #region Initialization

        public void Initialize()
        {
            InitializeCore();
            InitializeStateMachine();
            InitializeDataDependends();
            InitializeEvents();
            ApplySettings();

            InitializationFinished();
        }

        private void InitializeCore()
        {
            m_Dispatcher = Dispatcher.Instance;
            m_RhytmController = m_Dispatcher.GetController<RhytmController>();
            m_RhytmInputProxy = m_Dispatcher.GetController<RhytmInputProxy>();
            m_InputController = m_Dispatcher.GetController<InputController>();
            m_SpawnController = m_Dispatcher.GetController<SpawnController>();
            m_DamageController = m_Dispatcher.GetController<DamageController>();
            m_BattlefieldController = m_Dispatcher.GetController<BattlefieldController>();

            m_BattleModel = m_Dispatcher.GetModel<BattleModel>();
            m_WorldDataModel = m_Dispatcher.GetModel<WorldDataModel>();
            m_AccountDataModel = m_Dispatcher.GetModel<AccountDataModel>();
        }

        private void InitializeStateMachine()
        {
            m_StateMachine = new BattleStateMachine<BattleState_Abstract>();
            m_StateMachine.AddState(new BattleState_LockInput(m_RhytmInputProxy));
            m_StateMachine.AddState(new BattleState_Normal(m_RhytmInputProxy, m_BattlefieldController, m_DamageController));
            m_StateMachine.Initialize<BattleState_LockInput>();
        }

        private void InitializeDataDependends()
        {
            //Rhytm data
            int bpm = 30;
            m_RhytmController.SetBPM(bpm);
            m_RhytmInputProxy.SetInputPrecious(0.25f);//ManagersHolder.SettingsManager.GeneralSettings.InputPrecious);
            Metronome.bpm = bpm; //Debug

            //Initialize managers (May require data)
            MonoReferencesHolder.Initialize();

            //Build level data
            m_SpawnController.BuildLevel(MonoReferencesHolder.EntitySpawner, m_WorldDataModel.Areas[m_BattleModel.CurrentArea],
                m_RhytmController.CurrentTick, (float)m_AccountDataModel.CompletedLevels / m_WorldDataModel.Areas[m_BattleModel.CurrentArea].TotalLevels);
        }

        private void InitializeEvents()
        {
            //Input
            m_InputController.OnTouch += m_StateMachine.HandleTouch;

            //Level
            m_SpawnController.OnLevelComplete += LevelCompleteHandler;
            m_SpawnController.OnLevelFailed += LevelFailedHandler;

            //Rhytm
            m_RhytmController.OnEventProcessingTick += EventProcessingTickHandler;
            m_RhytmController.OnStarted += TickingStartedHandler;
            m_RhytmController.OnTick += TickHandler;
        }

        private void DisposeEvents()
        {
            //Input
            m_InputController.OnTouch -= m_StateMachine.HandleTouch;

            //Level
            m_SpawnController.OnLevelComplete += LevelCompleteHandler;
            m_SpawnController.OnLevelFailed += LevelFailedHandler;

            //Rhytm
            m_RhytmController.OnEventProcessingTick = null;
            m_RhytmController.OnStarted = null;
            m_RhytmController.OnTick = null;
        }

        private void ApplySettings()
        {
        }

        private void InitializationFinished()
        {
            StartCoroutine(TempStartCoroutine());
        }

        System.Collections.IEnumerator TempStartCoroutine()
        {
            yield return new WaitForSeconds(1);

            //TODO: Move to InitializationFinished and remove this

            //Enable input
            m_StateMachine.ChangeState<BattleState_Normal>();


            //Show UI
            MonoReferencesHolder.UIManager.ChangeState<UIBattleState_Normal>();

            //Start beat
            m_RhytmController.StartTicking();

            //Start player movement
            m_BattleModel.PlayerEntity.GetModule<MoveModule>().StartMove(Vector3.forward);
        }

        #endregion

        #region Runtime

        #region Rhytm

        private void TickingStartedHandler()
        {
            //Debug.Log("Tick started");
            //Metronome.StartMetronome();
        }

        private void TickHandler(int ticksSinceStart)
        {
            //Debug.Log("TickHandler: " + ticksSinceStart);
            if (ticksSinceStart % 8 == 0)
            {
                //Music.Play();
            }

            m_SpawnController.HandleTick(ticksSinceStart);
        }

        private void EventProcessingTickHandler(int ticksSinceStart)
        {
            //Debug.Log("EventProcessingTickHandler: " + ticksSinceStart);
        }

        #endregion

        #region Level

        private void LevelCompleteHandler()
        {
            LevelFinished();

            Debug.Log("Level complete");
        }

        private void LevelFailedHandler()
        {
            LevelFinished();

            Debug.Log("Level failed");
        }

        private void LevelFinished()
        {
            Dispatcher.Instance.GetModel<BattleModel>().PlayerEntity?.GetModule<MoveModule>().Stop();

            m_StateMachine.ChangeState<BattleState_LockInput>();
            MonoReferencesHolder.UIManager.ChangeState<UIBattleState_NoUI>();

            DisposeEvents();
        }

        #endregion


        #endregion
    }
}
