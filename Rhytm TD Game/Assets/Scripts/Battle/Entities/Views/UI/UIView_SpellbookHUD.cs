using CoreFramework.UI.View;
using CoreFramework.UI.Widget;
using RhytmTD.Battle.Entities;
using RhytmTD.Battle.Entities.Controllers;
using RhytmTD.Battle.Entities.Models;
using RhytmTD.Data.Models.DataTableModels;
using RhytmTD.UI.Widget;
using UnityEngine;

namespace RhytmTD.UI.Battle.View.UI
{
    /// <summary>
    /// Отображение виджетов боя в HUD
    /// </summary>
    public class UIView_SpellbookHUD : UIView_Abstract
    {
        [Space]
        [SerializeField] private RectTransform m_SpellsRoot = null;

        [Header("Widgets")]
        [SerializeField] private UIWidget_Button m_UIWidget_ButtonClose = null;
        [SerializeField] private UIWidget_SkillDirectionSelection m_UIWidget_SkillDirectionSelection = null;

        public UIWidget_SkillDirectionSelection ExposedUIWidget_SkillDirectionSelection => m_UIWidget_SkillDirectionSelection;

        private BattleModel m_BattleModel;
        private WorldDataModel m_WorldDataModel;
        private SpellBookController m_SpellBookController;

        public override void Initialize()
        {
            m_WorldDataModel = Dispatcher.GetModel<WorldDataModel>();
            m_BattleModel = Dispatcher.GetModel<BattleModel>();
            m_SpellBookController = Dispatcher.GetController<SpellBookController>();

            if (m_BattleModel.PlayerEntity == null)
                m_BattleModel.OnPlayerEntityInitialized += CreateSpellWidgets;
            else
                CreateSpellWidgets(m_BattleModel.PlayerEntity);

            m_UIWidget_SkillDirectionSelection.Initialize();
            RegisterWidget(m_UIWidget_SkillDirectionSelection);

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
            m_SpellBookController.CloseSpellBook();
        }
    }
}
