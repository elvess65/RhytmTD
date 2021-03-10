using RhytmTD.Animation.DOTween;
using UnityEngine;
using UnityEngine.UI;

namespace RhytmTD.Battle.Entities.Views
{
    public class EnemyHealthBarView : BattleEntityView
    {
        [SerializeField] private RectTransform m_Root = null;
        [SerializeField] private Image Foreground = null;
        [SerializeField] private DOTweenSequenceAnimator ShowDoTweenAnimator = null;
        [SerializeField] private DOTweenSequenceAnimator HideDoTweenAnimator = null;

        private HealthModule m_HealthModule;
        private RectTransform m_ForegroundRectTransform;
        private float m_FilledSize;

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

            m_ForegroundRectTransform = Foreground.rectTransform;
            m_FilledSize = m_Root.GetComponent<RectTransform>().sizeDelta.x;

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

            float progress = m_HealthModule.CurrentHealth / (float)m_HealthModule.Health;
            float damageOffset = m_FilledSize - progress * m_FilledSize;
            float damageHaldOffset = damageOffset / 2;

            m_ForegroundRectTransform.SetLeft(damageHaldOffset);
            m_ForegroundRectTransform.SetRight(damageHaldOffset);
        }

        private void RotationChanged(Quaternion rotation)
        {
            transform.rotation = Quaternion.LookRotation(Vector3.forward);
        }
    }
}
