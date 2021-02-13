using CoreFramework;
using RhytmTD.Battle.Entities.Models;
using UnityEngine;
using UnityEngine.UI;

namespace RhytmTD.Battle.Entities.Views
{
    public class PlayerHealthBarView : BaseView
    {
        [SerializeField] private Image Foreground;

        private HealthModule m_HealthModule;

        private void Awake()
        {
            BattleModel battleModel = Dispatcher.Instance.GetModel<BattleModel>();
            battleModel.OnPlayerEntityInitialized += PlayerInitialized;
        }

        private void PlayerInitialized(BattleEntity battleEntity)
        {
            m_HealthModule = battleEntity.GetModule<HealthModule>();
            m_HealthModule.OnHealthRemoved += HealthRemovedHandler;
        }

        public void HealthRemovedHandler(int health, int senderID)
        {
            Foreground.fillAmount = m_HealthModule.CurrentHealth / (float)m_HealthModule.Health;
        }
    }
}
