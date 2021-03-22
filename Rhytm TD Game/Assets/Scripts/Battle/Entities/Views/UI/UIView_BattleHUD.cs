using RhytmTD.Battle.Entities;
using RhytmTD.Battle.Entities.Controllers;
using RhytmTD.Battle.Entities.Models;
using RhytmTD.UI.View;
using RhytmTD.UI.Widget;
using UnityEngine;

namespace RhytmTD.UI.Battle.View.UI
{
    /// <summary>
    /// Отображение виджетов боя в HUD
    /// </summary>
    public class UIView_BattleHUD : UIView_Abstract
    {
        private HealthModule m_HealthModule;
        private BattleModel m_BattleModel;
        private SpellBookController m_SpellBookController;

        [Space]
        [SerializeField] private UIWidget_Metronome UIWidget_Metronome = null;
        [SerializeField] private UIWidget_PlayerHealthBar UIWidget_PlayerHealthBar = null;
        [SerializeField] private UIWidget_Button UIWidget_SpellBookButton = null;

        public UIWidget_Metronome ExposedUIWidget_Metronome => UIWidget_Metronome;


        public override void Initialize()
        {
            m_BattleModel = Dispatcher.GetModel<BattleModel>();
            m_SpellBookController = Dispatcher.GetController<SpellBookController>();

            if (m_BattleModel.PlayerEntity == null)
                m_BattleModel.OnPlayerEntityInitialized += PlayerInitialized;
            else
                PlayerInitialized(m_BattleModel.PlayerEntity);

            UIWidget_Metronome.Initialize();    
            RegisterWidget(UIWidget_Metronome);

            UIWidget_PlayerHealthBar.Initialize();
            RegisterWidget(UIWidget_PlayerHealthBar);

            UIWidget_SpellBookButton.Initialize();
            UIWidget_SpellBookButton.OnWidgetPress += SpellBookWidgetPressHandler;
            RegisterWidget(UIWidget_SpellBookButton);
        }

        private void PlayerInitialized(BattleEntity battleEntity)
        {
            m_HealthModule = battleEntity.GetModule<HealthModule>();
            m_HealthModule.OnHealthRemoved += HealthRemovedHandler;
            m_HealthModule.OnHealthRestored += HealthRestoredHandler;
        }

        private void HealthRemovedHandler(int health, int senderID)
        {
            UIWidget_PlayerHealthBar.UpdateHealthBar(m_HealthModule.CurrentHealth, m_HealthModule.Health);
            UIWidget_PlayerHealthBar.PlayDamageAnimation();
        }

        private void HealthRestoredHandler(int health)
        {
            UIWidget_PlayerHealthBar.UpdateHealthBar(m_HealthModule.CurrentHealth, m_HealthModule.Health);
            UIWidget_PlayerHealthBar.PlayHealAnimation();
        }

        private void SpellBookWidgetPressHandler()
        {
            m_SpellBookController.OpenSpellBook();
        }
    }
}
