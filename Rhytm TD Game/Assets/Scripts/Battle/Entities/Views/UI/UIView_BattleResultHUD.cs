using CoreFramework.SceneLoading;
using RhytmTD.Battle.Entities.Models;
using RhytmTD.Core;
using RhytmTD.UI.View;
using RhytmTD.UI.Widget;
using UnityEngine;

namespace RhytmTD.UI.Battle.View.UI
{
    /// <summary>
    /// Отображение виджетов окончания уровня
    /// </summary>
    public class UIView_BattleResultHUD : UIView_Abstract
    {
        private BattleModel m_BattleModel;

        [Header("Widgets")]
        [SerializeField] private UIWidget_BattleResult UIWidget_BattleResult = null;

        public override void Initialize()
        {
            UIWidget_BattleResult.Initialze();
            UIWidget_BattleResult.OnReplayButtonPressed += ReplayButtonPressedHandler;
            RegisterWidget(UIWidget_BattleResult);
        }

        private void ReplayButtonPressedHandler()
        {
            GameManager.Instance.SceneLoader.OnSceneUnloadingComplete += SceneUnloadedHandler;
            GameManager.Instance.SceneLoader.UnloadLevel(SceneLoader.BATTLE_SCENE_NAME);
        }

        private void SceneUnloadedHandler()
        {
            Dispatcher.Dispose();

            GameManager.Instance.SceneLoader.OnSceneUnloadingComplete -= SceneUnloadedHandler;
            GameManager.Instance.SceneLoader.LoadLevel(SceneLoader.BATTLE_SCENE_NAME);
        }
    }
}
