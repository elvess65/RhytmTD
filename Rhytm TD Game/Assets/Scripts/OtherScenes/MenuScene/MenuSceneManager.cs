﻿using CoreFramework;
using CoreFramework.Abstract;
using CoreFramework.SceneLoading;
using RhytmTD.Battle.Entities.Models;
using RhytmTD.Core;
using RhytmTD.Data.Models;
using RhytmTD.UI.View;
using UnityEngine;

namespace RhytmTD.OtherScenes.MenuScene
{
    public class MenuSceneManager : Singleton<MenuSceneManager>
    {
        [SerializeField] private UIView_MenuScene UIView = null;
        [SerializeField] private CameraContoller CameraContoller = null;
        [SerializeField] private CharactersController CharacterController = null;

        //private AnimationEventsListener m_SelectedCharacterAnimationEventsListener;


        public void Initialize()
        {
            UIView.OnPlayButtonPressHandler += UIView_PlayButton_PressHandler;
            UIView.Initialize();

            CharacterController.Initialize();

            CameraContoller.OnCameraPushKeyFrameReached += CameraPushingFinishedHandler;
            CameraContoller.Initialize();


            //Opened ares and completed levels are used to show player's best progress
            AccountDataModel accountDataModel = Dispatcher.Instance.GetModel<AccountDataModel>();
            Debug.Log($"Completed areas: {accountDataModel.CompletedAreas}. Completed levels: {accountDataModel.CompletedLevels}");

            //Here its possible to choose from amount of opened areas
            Dispatcher.Instance.GetModel<BattleModel>().CurrentArea = accountDataModel.CompletedAreas;

            //m_SelectedCharacterAnimationEventsListener = CharacterController.SelectedCharacterAnimationController.Controller.GetComponent<AnimationEventsListener>();
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