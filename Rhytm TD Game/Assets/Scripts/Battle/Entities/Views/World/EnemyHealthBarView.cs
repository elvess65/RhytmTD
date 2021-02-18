using UnityEngine;
using UnityEngine.UI;

namespace RhytmTD.Battle.Entities.Views
{
    public class EnemyHealthBarView : BattleEntityView
    {
        [SerializeField] private Image Foreground;

        private HealthModule m_HealthModule;

        public override void Initialize(BattleEntity battleEntity)
        {
            base.Initialize(battleEntity);

            TransformModule transformModule = battleEntity.GetModule<TransformModule>();
            transformModule.OnRotationChanged += RotationChanged;

            m_HealthModule = battleEntity.GetModule<HealthModule>();
            m_HealthModule.OnHealthRemoved += HealthRemoded;

            gameObject.SetActive(false);
        }

        private void HealthRemoded(int health, int senderID)
        {
            if (health >= 100)
                return;

            gameObject.SetActive(true);

            Foreground.fillAmount = m_HealthModule.CurrentHealth / (float)m_HealthModule.Health;
        }

        private void RotationChanged(Quaternion rotation)
        {
            transform.rotation = Quaternion.LookRotation(Vector3.forward);
        }
    }
}
