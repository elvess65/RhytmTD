﻿using RhytmTD.Animation.DOTween;
using RhytmTD.UI.Components;
using UnityEngine;

namespace RhytmTD.Battle.Entities.Views
{
    public class EnemyHealthBarView : BattleEntityView
    {
        [SerializeField] private RectTransform m_Root = null;
        [SerializeField] private UIComponent_CenteredBar m_UIComponent_CenteredBar = null;
        
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

            m_UIComponent_CenteredBar.Initialize(m_Root);

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
            if (m_HealthModule.CurrentHealth >= m_HealthModule.TotalHealth)
                return;

            if (m_HealthModule.CurrentHealth > 0 && !gameObject.activeSelf)
            {
                gameObject.SetActive(true);
                ShowDoTweenAnimator.PlaySequence();
            }

            m_UIComponent_CenteredBar.UpdateBar(m_HealthModule.CurrentHealth / (float)m_HealthModule.TotalHealth);
        }

        private void RotationChanged(Quaternion rotation)
        {
            transform.rotation = Quaternion.LookRotation(Vector3.forward);
        }
    }
}
