using CoreFramework;
using CoreFramework.Abstract;
using CoreFramework.Utils;
using RhytmTD.Battle.Entities.Models;
using RhytmTD.Battle.StateMachine;
using RhytmTD.Core;
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
        private ControllersHolder m_ControllersHolder;
        private UpdatablesManager m_UpdatablesManager;

        private void Update()
        {
            m_UpdatablesManager?.PerformUpdate(Time.deltaTime);
        }

        #region Initialization

        public void Initialize()
        {
            InitializeCore();
            InitializeStateMachine();
            InitializeDataDependends();
            InitializeUpdatables();
            InitializeEvents();
            ApplySettings();

            InitializationFinished();
        }

        private void InitializeCore()
        {
            m_ControllersHolder = new ControllersHolder();
        }

        private void InitializeStateMachine()
        {
            m_StateMachine = new BattleStateMachine<BattleState_Abstract>();
            m_StateMachine.AddState(new BattleState_LockInput(m_ControllersHolder.RhytmInputProxy));
            m_StateMachine.AddState(new BattleState_Normal(m_ControllersHolder.RhytmInputProxy));
            m_StateMachine.Initialize<BattleState_LockInput>();
        }

        private void InitializeDataDependends()
        {
            //Rhytm data
            int bpm = 30;
            m_ControllersHolder.RhytmController.SetBPM(bpm);
            m_ControllersHolder.RhytmInputProxy.SetInputPrecious(0.25f);//ManagersHolder.SettingsManager.GeneralSettings.InputPrecious);
            Metronome.bpm = bpm; //Debug

            //Initialize managers (May require data)
            MonoReferencesHolder.Initialize();

            //Build level data
            //Get current area id from account
            WorldDataModel.AreaData areaData = Dispatcher.Instance.GetModel<WorldDataModel>().Areas[Dispatcher.Instance.GetModel<BattleModel>().CurrentArea];
            m_ControllersHolder.SpawnController.BuildLevel(MonoReferencesHolder.EnemySpawner, areaData, m_ControllersHolder.RhytmController.CurrentTick);
        }

        private void InitializeUpdatables()
        {
            m_UpdatablesManager = new UpdatablesManager();
            m_UpdatablesManager.Add(m_ControllersHolder.RhytmController);
            m_UpdatablesManager.Add(m_ControllersHolder.InputController);
            m_UpdatablesManager.Add(MonoReferencesHolder.UIManager);
            m_UpdatablesManager.Add(m_StateMachine);
        }

        private void InitializeEvents()
        {
            //Input
            m_ControllersHolder.InputController.OnTouch += m_StateMachine.HandleTouch;

            //Rhytm
            m_ControllersHolder.RhytmController.OnEventProcessingTick += EventProcessingTickHandler;
            m_ControllersHolder.RhytmController.OnStarted += TickingStartedHandler;
            m_ControllersHolder.RhytmController.OnTick += TickHandler;
        }

        private void DisposeEvents()
        {
            //Input
            m_ControllersHolder.InputController.OnTouch -= m_StateMachine.HandleTouch;

            //Rhytm
            m_ControllersHolder.RhytmController.OnEventProcessingTick = null;
            m_ControllersHolder.RhytmController.OnStarted = null;
            m_ControllersHolder.RhytmController.OnTick = null;
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
            m_ControllersHolder.RhytmController.StartTicking();
        }

        #endregion

        #region Runtime

        #region Rhytm
        private void TickingStartedHandler()
        {
            //Debug.Log("Tick started");
            Metronome.StartMetronome();
        }

        private void TickHandler(int ticksSinceStart)
        {
            //Debug.Log("TickHandler: " + ticksSinceStart);
            if (ticksSinceStart % 8 == 0)
            {
                //Music.Play();
            }

            m_ControllersHolder.SpawnController.HandleTick(ticksSinceStart);
        }

        private void EventProcessingTickHandler(int ticksSinceStart)
        {
            //Debug.Log("EventProcessingTickHandler: " + ticksSinceStart);
        }
        #endregion

        #endregion
    }
}
