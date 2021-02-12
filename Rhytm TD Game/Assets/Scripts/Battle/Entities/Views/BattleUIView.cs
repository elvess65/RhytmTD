using CoreFramework;
using RhytmTD.Battle.Entities.Models;
using RhytmTD.UI.Battle.View.UI;
using UnityEngine;

namespace RhytmTD.Battle.Entities.Views
{
    public class BattleUIView : BaseView
    {
        [SerializeField] private UIView_BattleHUD m_UIView_BattleHUD;
        [SerializeField] private UIView_BattleResultHUD m_UIView_BattleResultHUD;

        private BattleUIModel m_UIModel;

        private void Awake()
        {
            Initialize();
        }

        private void Initialize()
        {
            m_UIModel = Dispatcher.GetModel<BattleUIModel>();

            m_UIModel.UIView_BattleHUD = m_UIView_BattleHUD;
            m_UIModel.UIView_BattleResultHUD = m_UIView_BattleResultHUD;
        }
    }
}
