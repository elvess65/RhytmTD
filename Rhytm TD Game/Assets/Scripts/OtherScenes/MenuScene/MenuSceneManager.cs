//using RhytmFighter.Animation;
using CoreFramework.Abstract;
using CoreFramework.SceneLoading;
using RhytmTD.Core;
using RhytmTD.UI.View;
//using RhytmFighter.UI.View;
using UnityEngine;

namespace RhytmTD.OtherScenes.MenuScene
{
    public class MenuSceneManager : Singleton<MenuSceneManager>
    {
        [SerializeField] private UIView_MenuScene UIView;
        [SerializeField] private CameraContoller CameraContoller;
        [SerializeField] private CharactersController CharacterController;

        //private AnimationEventsListener m_SelectedCharacterAnimationEventsListener;


        public void Initialize()
        {
            UIView.OnPlayButtonPressHandler += UIView_PlayButton_PressHandler;
            UIView.Initialize();

            CharacterController.Initialize();

            CameraContoller.OnCameraPushKeyFrameReached += CameraPushingFinishedHandler;
            CameraContoller.Initialize();


            //Opened ares and completed levels are used to show player's best progress
            Debug.Log("Completed areas: " + GameManager.Instance.ModelsHolder.AccountModel.CompletedAreas);
            Debug.Log("Completed levels: " + GameManager.Instance.ModelsHolder.AccountModel.CompletedLevels);

            //Here its possible to choose from amount of opened areas
            GameManager.Instance.ModelsHolder.BattleSessionModel.CurrentArea = GameManager.Instance.ModelsHolder.AccountModel.CompletedAreas;

            //m_SelectedCharacterAnimationEventsListener = CharacterController.SelectedCharacterAnimationController.Controller.GetComponent<AnimationEventsListener>();

            //GameManager.Instance.DataHolder.BattleSessionModel.CurrentLevelID = 1;
            //GameManager.Instance.DataHolder.BattleSessionModel.SelectedCharactedID = 1;
        }


        private void Update()
        {
            UIView.PerformUpdate(Time.deltaTime);
            CameraContoller.PerformUpdate(Time.deltaTime);
        }

 
        #region Button Play

        private void UIView_PlayButton_PressHandler()
        {
            //Camera is pushed by animation event

            //Disable all UI widgets
            UIView.SetWidgetsActive(false, true);

            CameraPushingFinishedHandler();//Remove

            //Subscribe for animation event
            //m_SelectedCharacterAnimationEventsListener.OnOtherEvent += CameraContoller.PushCamera;

            //Play animation
            //CharacterController.SelectedCharacterAnimationController.PlayAnimation(Persistant.Enums.AnimationTypes.MenuAction);
        }

        private void CameraPushingFinishedHandler()
        {
            GameManager.Instance.SceneLoader.OnSceneUnloadingComplete += ButtonPlay_MenuSceneUnloadedHandler;
            GameManager.Instance.SceneLoader.UnloadLevel(SceneLoader.MENU_SCENE_NAME);
        }

        private void ButtonPlay_MenuSceneUnloadedHandler()
        {
            GameManager.Instance.SceneLoader.OnSceneUnloadingComplete -= ButtonPlay_MenuSceneUnloadedHandler;
            GameManager.Instance.SceneLoader.LoadLevel(SceneLoader.BATTLE_SCENE_NAME);
        }

        #endregion
    }
}