using CoreFramework.Rhytm;
using RhytmTD.Battle.Entities;
using RhytmTD.Battle.Entities.Controllers;
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
        private WorldDataModel m_WorldDataModel;
        private RhytmController m_RhytmController;
        private PrepareSkillUseController m_PrepareSkillUseController;

        [Space]
        [SerializeField] private UIWidget_Button m_UIWidget_ButtonClose;
        [SerializeField] private RectTransform m_SpellsRoot;

        public override void Initialize()
        {
            m_BattleModel = Dispatcher.GetModel<BattleModel>();
            m_WorldDataModel = Dispatcher.GetModel<WorldDataModel>();
            m_RhytmController = Dispatcher.GetController<RhytmController>();
            m_PrepareSkillUseController = Dispatcher.GetController<PrepareSkillUseController>();

            m_UIWidget_ButtonClose.Initialize();
            m_UIWidget_ButtonClose.OnWidgetPress += ButtonCloseWidgetPressHandler;
            RegisterWidget(m_UIWidget_ButtonClose);

            CreateSpellWidgets();
        }

        private void CreateSpellWidgets()
        {
            LoadoutModule playerLodouatModule = m_BattleModel.PlayerEntity.GetModule<LoadoutModule>();
            foreach(int skillTypeID in playerLodouatModule.SelectedSkillTypeIDs)
            {
                
                UIWidget_Spell spellWidget = m_WorldDataModel.UIAssets.InstantiatePrefab(m_WorldDataModel.UIAssets.UIWidgetSpellPrefab) as UIWidget_Spell;
                spellWidget.transform.parent = m_SpellsRoot;

                spellWidget.Initialize(skillTypeID, playerLodouatModule.GetSkillIDByTypeID(skillTypeID));
                spellWidget.OnSpellUse += SpellUseHandler;
                RegisterWidget(spellWidget);
            }
        }

        private void SpellUseHandler(int skillID)
        {
            m_PrepareSkillUseController.StartUseSkill(skillID);
            m_BattleModel.OnSpellbookExit?.Invoke();
        }

        private void ButtonCloseWidgetPressHandler()
        {
            m_BattleModel.OnSpellbookExit?.Invoke();
        }
    }
}
