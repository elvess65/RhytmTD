using CoreFramework;
using CoreFramework.Abstract;
using CoreFramework.Utils;
using RhytmTD.Battle.StateMachine;
using System.Collections.Generic;
using UnityEngine;

namespace RhytmTD.Battle.Core
{
    public class BattleManager : Singleton<BattleManager>
    {
        public ManagersHolder ManagersHolder;

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
            //EnvironmentDataModel.LevelParams levelParams = GameManager.Instance.ModelsHolder.DataTableModel.EnvironmentDataModel.GetLevelParams(GameManager.Instance.ModelsHolder.BattleSessionModel.CurrentLevelID);
            //float completionProgress = GameManager.Instance.ModelsHolder.DataTableModel.EnvironmentDataModel.GetCompletionForProgression(GameManager.Instance.ModelsHolder.BattleSessionModel.CompletedLevelsIDs.ToArray());

            //Rhytm data
            int bpm = 130;
            m_ControllersHolder.RhytmController.SetBPM(bpm);
            m_ControllersHolder.RhytmInputProxy.SetInputPrecious(0.25f);//ManagersHolder.SettingsManager.GeneralSettings.InputPrecious);
            Metronome.bpm = bpm; //Debug

            //Initialize managers (May require data)
            ManagersHolder.Initialize();
        }

        private void InitializeUpdatables()
        {
            m_UpdatablesManager = new UpdatablesManager();
            m_UpdatablesManager.Add(m_ControllersHolder.RhytmController);
            m_UpdatablesManager.Add(m_ControllersHolder.InputController);
            m_UpdatablesManager.Add(ManagersHolder.UIManager);
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
            //ManagersHolder.UIManager.ShowUI

            //Start beat
            m_ControllersHolder.RhytmController.StartTicking();
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
            Debug.Log("TickHandler: " + ticksSinceStart);
            if (ticksSinceStart % 8 == 0)
            {
                //Music.Play();
            }
        }

        private void EventProcessingTickHandler(int ticksSinceStart)
        {
            //Debug.Log("EventProcessingTickHandler: " + ticksSinceStart);
        }
        #endregion

        #endregion
    }
}
