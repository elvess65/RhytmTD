using CoreFramework.UI.Widget;
using RhytmTD.Battle.Entities.Models;
using UnityEngine;
using UnityEngine.UI;

namespace RhytmTD.UI.Widget
{
    /// <summary>
    /// Виджет отображения окончания уровня
    /// </summary>
    public class UIWidget_BattleResult : UIWidget
    {
        public System.Action OnReplayButtonPressed;

        private BattleModel m_BattleModel;

        [Space(10)]
        [SerializeField] private Text Text_BattleResult = null;
        [SerializeField] private Image Image_BattleResult = null;
        [SerializeField] private Button Button_Replay = null;

        public void Initialze()
        {
            m_BattleModel = Dispatcher.GetModel<BattleModel>();
            m_BattleModel.OnBattleFinished += ShowBattleResult;

            Button_Replay.onClick.AddListener(ButtonReplayPressHandler);

            InternalInitialize();
        }

        private void ShowBattleResult(bool isSuccess)
        {
            Text_BattleResult.text = isSuccess ? "Success" : "Game Over";
            Text_BattleResult.color = isSuccess ? Color.green : Color.red;
            Image_BattleResult.color = isSuccess ? Color.green : Color.red;
        }

        private void ButtonReplayPressHandler()
        {
            OnReplayButtonPressed?.Invoke();
        }

        public override void Dispose()
        {
            base.Dispose();

            m_BattleModel.OnBattleFinished -= ShowBattleResult;
        }
    }
}
