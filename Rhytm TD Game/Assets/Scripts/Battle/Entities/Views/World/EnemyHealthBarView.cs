using RhytmTD.Animation.DOTween;
using UnityEngine;
using UnityEngine.UI;

namespace RhytmTD.Battle.Entities.Views
{
    public class EnemyHealthBarView : BattleEntityView
    {
        [SerializeField] private Image Foreground = null;
        [SerializeField] private DOTweenSequenceAnimator ShowDoTweenAnimator = null;
        [SerializeField] private DOTweenSequenceAnimator HideDoTweenAnimator = null;

        private HealthModule m_HealthModule;

        public override void Initialize(BattleEntity battleEntity)
        {
            base.Initialize(battleEntity);

            TransformModule transformModule = battleEntity.GetModule<TransformModule>();
            transformModule.OnRotationChanged += RotationChanged;

            transform.rotation = Quaternion.LookRotation(Vector3.forward);

            DestroyModule destroyModule = battleEntity.GetModule<DestroyModule>();
            destroyModule.OnDestroyed += OnDestroyed;

            m_HealthModule = battleEntity.GetModule<HealthModule>();
            m_HealthModule.OnHealthRemoved += HealthRemoved;

            gameObject.SetActive(false);
        }

        private void OnDestroyed(BattleEntity entity)
        {
            if (gameObject.activeSelf)
            {
                HideDoTweenAnimator.PlaySequence(OnHealthBarHideAnimationComplete);
            }
        }

        private void OnHealthBarHideAnimationComplete()
        {
            gameObject.SetActive(false);
        }

        private void HealthRemoved(int health, int senderID)
        {
            if (m_HealthModule.CurrentHealth >= m_HealthModule.Health)
                return;

            if (m_HealthModule.CurrentHealth > 0 && !gameObject.activeSelf)
            {
                gameObject.SetActive(true);
                ShowDoTweenAnimator.PlaySequence();
            }

            Foreground.fillAmount = m_HealthModule.CurrentHealth / (float)m_HealthModule.Health;
        }

        private void RotationChanged(Quaternion rotation)
        {
            transform.rotation = Quaternion.LookRotation(Vector3.forward);
        }
    }
}
