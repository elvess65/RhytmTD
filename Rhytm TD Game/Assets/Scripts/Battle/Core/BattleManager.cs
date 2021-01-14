using RhytmTD.Persistant.Abstract;
using System.Collections.Generic;
using UnityEngine;

namespace RhytmTD.Battle.Core
{
    public class BattleManager : Singleton<BattleManager>
    {
        public ManagersHolder ManagersHolder;
        public FrameworkPackage.Utils.Metronome Metronome;
        public AudioSource Music;

        private ControllersHolder m_ControllersHolder;
        private List<iUpdatable> m_Updateables;

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
                m_ControllersHolder.RhytmController
            };
        }

        private void InitializeEvents()
        {
            //Rhytm
            m_ControllersHolder.RhytmController.OnEventProcessingTick += EventProcessingTickHandler;
            m_ControllersHolder.RhytmController.OnStarted += TickingStartedHandler;
            m_ControllersHolder.RhytmController.OnTick += TickHandler;
        }

        private void DisposeEvents()
        {
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

        #region Rhytm
        private void TickingStartedHandler()
        {
            Debug.Log("Tick started");
            Metronome.StartMetronome();
        }

        private void TickHandler(int ticksSinceStart)
        {
            Debug.Log("TickHandler: " + ticksSinceStart);
            if (ticksSinceStart % 8 == 0)
            {
                Music.Play();
            }
        }

        private void EventProcessingTickHandler(int ticksSinceStart)
        {
            Debug.Log("EventProcessingTickHandler: " + ticksSinceStart);
        }
        #endregion
    }
}
