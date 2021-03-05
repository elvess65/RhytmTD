using CoreFramework.Rhytm;
using RhytmTD.Battle.Entities;
using RhytmTD.Battle.Entities.Models;
using RhytmTD.Data.Models.DataTableModels;
using RhytmTD.UI.View;
using RhytmTD.UI.Widget;
using UnityEngine;

namespace RhytmTD.UI.Battle.View.UI
{
    /// <summary>
    /// Отображение виджетов боя в HUD
    /// </summary>
    public class UIView_SpellbookHUD : UIView_Abstract
    {
        private BattleModel m_BattleModel;
        private SkillsModel m_SkillsModel;
        private WorldDataModel m_WorldDataModel;

        [Space]
        [SerializeField] private UIWidget_Button m_UIWidget_ButtonClose = null;
        [SerializeField] private RectTransform m_SpellsRoot = null;

        public override void Initialize()
        {
            m_SkillsModel = Dispatcher.GetModel<SkillsModel>();
            m_WorldDataModel = Dispatcher.GetModel<WorldDataModel>();

            m_BattleModel = Dispatcher.GetModel<BattleModel>();

            if (m_BattleModel.PlayerEntity == null)
                m_BattleModel.OnPlayerEntityInitialized += CreateSpellWidgets;
            else
                CreateSpellWidgets(m_BattleModel.PlayerEntity);

            m_UIWidget_ButtonClose.Initialize();
            m_UIWidget_ButtonClose.OnWidgetPress += ButtonCloseWidgetPressHandler;
            RegisterWidget(m_UIWidget_ButtonClose);
        }

        private void CreateSpellWidgets(BattleEntity entity)
        {
            LoadoutModule playerLodouatModule = entity.GetModule<LoadoutModule>();
            foreach(int skillTypeID in playerLodouatModule.SelectedSkillTypeIDs)
            {
                UIWidget_Spell spellWidget = m_WorldDataModel.UIAssets.InstantiatePrefab(m_WorldDataModel.UIAssets.UIWidgetSpellPrefab);
                spellWidget.transform.SetParent(m_SpellsRoot);
                spellWidget.transform.localPosition = Vector3.zero;

                spellWidget.Initialize(skillTypeID, playerLodouatModule.GetSkillIDByTypeID(skillTypeID));
                RegisterWidget(spellWidget);
            }
        }

        private void ButtonCloseWidgetPressHandler()
        {
            m_BattleModel.OnSpellbookClosed?.Invoke();
        }
    }
}
