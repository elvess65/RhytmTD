using CoreFramework.Abstract;
using CoreFramework.Utils;
using RhytmTD.Battle.StateMachine;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace RhytmTD.Battle.Core
{
    public class BattleManager : Singleton<BattleManager>
    {
        public ManagersHolder ManagersHolder;
        public Metronome Metronome;
        public AudioSource Music;

        private BattleStateMachine m_StateMachine;
        private ControllersHolder m_ControllersHolder;
        private List<iUpdatable> m_Updateables;
        private Dictionary<Type, BattleState_Abstract> m_InitializedStates;

        private void Update()
        {
            if (m_Updateables != null)
            {
                for (int i = 0; i < m_Updateables.Count; i++)
                    m_Updateables[i].PerformUpdate(Time.deltaTime);
            }
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
            m_StateMachine = new BattleStateMachine();
            m_InitializedStates = new Dictionary<Type, BattleState_Abstract>();

            m_InitializedStates.Add(typeof(BattleState_LockInput), new BattleState_LockInput(null));
            m_StateMachine.Initialize(m_InitializedStates[typeof(BattleState_LockInput)]);
        }

        private void InitializeDataDependends()
        {
            //EnvironmentDataModel.LevelParams levelParams = GameManager.Instance.ModelsHolder.DataTableModel.EnvironmentDataModel.GetLevelParams(GameManager.Instance.ModelsHolder.BattleSessionModel.CurrentLevelID);
            //float completionProgress = GameManager.Instance.ModelsHolder.DataTableModel.EnvironmentDataModel.GetCompletionForProgression(GameManager.Instance.ModelsHolder.BattleSessionModel.CompletedLevelsIDs.ToArray());

            //Rhytm data
            m_ControllersHolder.RhytmController.SetBPM(65);
            //m_ControllersHolder.RhytmInputProxy.SetInputPrecious(ManagersHolder.SettingsManager.GeneralSettings.InputPrecious);

            //Initialize managers (May require data)
            ManagersHolder.Initialize();
        }

        private void InitializeUpdatables()
        {
            m_Updateables = new List<iUpdatable>
            {
                m_ControllersHolder.RhytmController,
                m_ControllersHolder.InputController,
                m_StateMachine
            };
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
            //Debug.Log("TickHandler: " + ticksSinceStart);
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

        public void ChangeState<T>() where T : BattleState_Abstract
        {
            m_StateMachine.ChangeState(m_InitializedStates[typeof(T)]);
        }

        #endregion
    }
}
