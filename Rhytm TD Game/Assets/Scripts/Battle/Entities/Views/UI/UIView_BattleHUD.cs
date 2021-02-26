using CoreFramework.Rhytm;
using RhytmTD.Battle.Entities;
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
        private RhytmController m_RhytmController;
        private HealthModule m_HealthModule;
        private BattleModel m_BattleModel;

        [Space]
        [SerializeField] private UIWidget_Metronome UIWidget_Metronome;
        [SerializeField] private UIWidget_PlayerHealthBar UIWidget_PlayerHealthBar;

        public override void Initialize()
        {
            m_BattleModel = Dispatcher.GetModel<BattleModel>();
            if (m_BattleModel.PlayerEntity == null)
                m_BattleModel.OnPlayerEntityInitialized += PlayerInitialized;
            else
                PlayerInitialized(m_BattleModel.PlayerEntity);

            m_RhytmController = Dispatcher.GetController<RhytmController>();

            UIWidget_Metronome.Initialize();    
            RegisterWidget(UIWidget_Metronome);

            UIWidget_PlayerHealthBar.Initialize();
            RegisterWidget(UIWidget_PlayerHealthBar);
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
    }
}
