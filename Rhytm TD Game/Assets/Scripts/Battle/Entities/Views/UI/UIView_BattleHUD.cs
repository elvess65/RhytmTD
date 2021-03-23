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
        private BattleModel m_BattleModel;
        private SpellBookController m_SpellBookController;

        private ManaModule m_ManaModule;
        private HealthModule m_HealthModule;

        [Space]
        [SerializeField] private UIWidget_Metronome UIWidget_Metronome = null;
        [SerializeField] private UIWidget_PlayerHealthBar UIWidget_PlayerHealthBar = null;
        [SerializeField] private UIWidget_PlayerManaBar UIWidget_PlayerManaBar = null;
        [SerializeField] private UIWidget_Button UIWidget_SpellBookButton = null;

        public UIWidget_Metronome ExposedUIWidget_Metronome => UIWidget_Metronome;


        public override void Initialize()
        {
            m_BattleModel = Dispatcher.GetModel<BattleModel>();
            m_SpellBookController = Dispatcher.GetController<SpellBookController>();

            if (m_BattleModel.PlayerEntity == null)
                m_BattleModel.OnPlayerEntityInitialized += PlayerInitializedHandler;
            else
                PlayerInitializedHandler(m_BattleModel.PlayerEntity);

            UIWidget_Metronome.Initialize();    
            RegisterWidget(UIWidget_Metronome);

            UIWidget_PlayerHealthBar.Initialize();
            RegisterWidget(UIWidget_PlayerHealthBar);

            UIWidget_PlayerManaBar.Initialize();
            RegisterWidget(UIWidget_PlayerManaBar);

            UIWidget_SpellBookButton.Initialize();
            UIWidget_SpellBookButton.OnWidgetPress += SpellBookWidgetPressHandler;
            RegisterWidget(UIWidget_SpellBookButton);
        }


        private void PlayerInitializedHandler(BattleEntity battleEntity)
        {
            m_HealthModule = battleEntity.GetModule<HealthModule>();
            m_HealthModule.OnHealthRemoved += HealthRemovedHandler;
            m_HealthModule.OnHealthAdded += HealthRestoredHandler;

            m_ManaModule = battleEntity.GetModule<ManaModule>();
            m_ManaModule.OnManaAdded += ManaAddedHandler;
            m_ManaModule.OnManaRemoved += ManaRemovedHandler;
        }


        private void HealthRemovedHandler(int health, int senderID)
        {
            UIWidget_PlayerHealthBar.UpdateBar(m_HealthModule.CurrentHealth, m_HealthModule.Health);
            UIWidget_PlayerHealthBar.PlayRemoveAnimation();
        }

        private void HealthRestoredHandler(int health)
        {
            UIWidget_PlayerHealthBar.UpdateBar(m_HealthModule.CurrentHealth, m_HealthModule.Health);
            UIWidget_PlayerHealthBar.PlayAddAnimation();
        }

        private void ManaAddedHandler(int mana)
        {
            Debug.Log("Mana added: " + mana);
        }

        private void ManaRemovedHandler(int mana)
        {
            Debug.Log("Mana removed: " + mana);
        }



        private void SpellBookWidgetPressHandler()
        {
            m_SpellBookController.OpenSpellBook();
        }
    }
}
