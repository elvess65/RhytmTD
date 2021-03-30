using RhytmTD.Battle.Entities.Models;
using RhytmTD.Core;
using RhytmTD.OtherScenes.MenuScene;
using UnityEngine.SceneManagement;

namespace CoreFramework.SceneLoading
{
    public partial class SceneLoader
    {
        public const string BATTLE_SCENE_NAME = "BattleScene";
        public const string MENU_SCENE_NAME = "MenuScene";

        private const string m_TRANSITION_SCENE_NAME = "TransitionScene";
        private const string m_BOOT_SCENE_NAME = "BootScene";

        private BattleModel m_BattleModel;

        partial void PartialConstructorCall()
        {
            LoadLevel(m_TRANSITION_SCENE_NAME);
        }

        partial void SceneLoadCompleteHandler()
        {
            switch (m_CurrentLoadingLevel)
            {
                case MENU_SCENE_NAME:
                    SceneManager.SetActiveScene(SceneManager.GetSceneByName(MENU_SCENE_NAME));
                    SceneLoadingManager.Instance.FadeOut();
                    MenuSceneManager.Instance.Initialize();
                    break;

                case BATTLE_SCENE_NAME:

                    SceneManager.SetActiveScene(SceneManager.GetSceneByName(BATTLE_SCENE_NAME));
                    SceneLoadingManager.Instance.FadeOut();

                    if (m_BattleModel == null)
                        m_BattleModel = Dispatcher.GetModel<BattleModel>();

                    m_BattleModel.OnBattleInitialize();

                    break;

                case m_TRANSITION_SCENE_NAME:
                    GameManager.Instance.InitializeConnection();
                    break;
            }
}
    }
}
