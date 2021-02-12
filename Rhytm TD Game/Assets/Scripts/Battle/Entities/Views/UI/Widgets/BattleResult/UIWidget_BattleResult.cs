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
        private BattleModel m_BattleModel;

        [Space(10)]
        [SerializeField] private Text Text_BattleResult;
        [SerializeField] private Image Image_BattleResult;
        [SerializeField] private Button Button_Replay;

        public void Initialze()
        {
            m_BattleModel = Dispatcher.GetModel<BattleModel>();
            m_BattleModel.OnBattleFinished += ShowBattleResult;

            Button_Replay.onClick.AddListener(ButtonReplayPressHandler);
        }

        private void ShowBattleResult(bool isSuccess)
        {
            Text_BattleResult.text = isSuccess ? "Success" : "Game Over";
            Text_BattleResult.color = isSuccess ? Color.green : Color.red;
            Image_BattleResult.color = isSuccess ? Color.green : Color.red;
        }

        private void ButtonReplayPressHandler()
        {
            Debug.Log("1");
        }

        protected override void Dispose()
        {
            base.Dispose();

            m_BattleModel.OnBattleFinished -= ShowBattleResult;
        }
    }
}
