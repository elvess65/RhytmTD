using RhytmTD.Battle.Entities.Models;
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
        [SerializeField] private UIWidget_BattleResult UIWidget_BattleResult;

        public override void Initialize()
        {
            UIWidget_BattleResult.Initialze();
            RegisterWidget(UIWidget_BattleResult);
        }
    }
}
