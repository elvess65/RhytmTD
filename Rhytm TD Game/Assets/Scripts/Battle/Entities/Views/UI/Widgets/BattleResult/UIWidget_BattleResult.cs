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

        public void Initialze()
        {
            m_BattleModel = Dispatcher.GetModel<BattleModel>();
            m_BattleModel.OnBattleFinished += ShowBattleResult;
        }

        private void ShowBattleResult(bool isSuccess)
        {
            Text_BattleResult.text = isSuccess ? "Success" : "Game Over";
            Text_BattleResult.color = isSuccess ? Color.green : Color.red;
            Image_BattleResult.color = isSuccess ? Color.green : Color.red;
        }

        protected override void Dispose()
        {
            base.Dispose();

            m_BattleModel.OnBattleFinished -= ShowBattleResult;
        }
    }
}
