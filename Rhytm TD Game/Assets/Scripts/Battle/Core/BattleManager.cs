using CoreFramework;
using CoreFramework.Abstract;
using CoreFramework.Rhytm;
using CoreFramework.Utils;
using RhytmTD.Battle.Entities;
using RhytmTD.Battle.Entities.Controllers;
using RhytmTD.Battle.Entities.Models;
using RhytmTD.Battle.Spawn;
using RhytmTD.Battle.StateMachine;
using RhytmTD.UI.Battle.StateMachine;
using UnityEngine;

namespace RhytmTD.Battle.Core
{
    public class BattleManager : Singleton<BattleManager>
    {
        private Dispatcher m_Dispatcher;
        private RhytmInputProxy m_RhytmInputProxy;
        private RhytmController m_RhytmController;
        private SpawnController m_SpawnController;
        private DamageController m_DamageController;

        private BattleModel m_BattleModel;

        public BattleStateMachine<BattleState_Abstract> StateMachine { get; private set; }

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
            m_SpawnController = m_Dispatcher.GetController<SpawnController>();
            m_DamageController = m_Dispatcher.GetController<DamageController>();

            m_BattleModel = m_Dispatcher.GetModel<BattleModel>();
        }

        private void InitializeStateMachine()
        {
            StateMachine = new BattleStateMachine<BattleState_Abstract>();
            StateMachine.AddState(new BattleState_LockInput());
            StateMachine.AddState(new BattleState_Normal());
            StateMachine.Initialize<BattleState_LockInput>();
        }

        private void InitializeDataDependends()
        {
            //Rhytm data
            Dispatcher.Instance.GetModel<BattleAudioModel>().BPM = 30;

            m_RhytmInputProxy.SetInputPrecious(0.25f);

            //Build level data
            m_SpawnController.BuildLevel();
        }

        private void InitializeEvents()
        {
            //Level
            m_SpawnController.OnLevelComplete += LevelCompleteHandler;
            m_SpawnController.OnLevelFailed += LevelFailedHandler;
        }

        private void DisposeEvents()
        {
            //Level
            m_SpawnController.OnLevelComplete -= LevelCompleteHandler;
            m_SpawnController.OnLevelFailed -= LevelFailedHandler;
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

            m_BattleModel.OnBattleStarted?.Invoke();

            //TODO: Move to InitializationFinished and remove this

            //Enable input
            StateMachine.ChangeState<BattleState_Normal>();


            //Show UI
            //MonoReferencesHolder.UIManager.ChangeState<UIBattleState_Normal>();

            //Start beat
            m_RhytmController.StartTicking();

            Dispatcher.Instance.GetModel<BattleAudioModel>().OnPlayMetronome(true);
            //Dispatcher.Instance.GetModel<BattleAudioModel>().OnPlayMusic(true);

            //Start player movement
            m_BattleModel.PlayerEntity.GetModule<MoveModule>().StartMove(Vector3.forward);
        }

        #endregion

        #region Runtime

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

            StateMachine.ChangeState<BattleState_LockInput>();
            //MonoReferencesHolder.UIManager.ChangeState<UIBattleState_NoUI>();

            DisposeEvents();
        }

        #endregion


        #endregion
    }
}
