using CoreFramework;
using DG.Tweening;
using RhytmTD.Animation.DOTween;
using RhytmTD.Battle.Entities.Models;
using UnityEngine;

namespace RhytmTD.Battle.Entities.Views.Effects
{
    public class PlayerAppearPortalEffectView : BaseView
    {
        [Header("Portal")]
        public MeshRenderer PortalRenderer;
        public DOTweenSequenceAnimator PortalShowAnimator;
        public DOTweenSequenceAnimator PortalHideAnimator;

        [Header("Vortex")]
        public ParticleSystem VortexEffect;
        public DOTweenSequenceAnimator VortexShowAnimator;
        public DOTweenSequenceAnimator VortexHideAnimator;

        [Header("Spawn")]
        public ParticleSystem[] SpawnEffects;
        public Transform FromPortalPos;

        private BattleModel m_BattleModel;
        private StartBattleSequenceModel m_StartBattleSequenceModel;

        private const string m_PORTAL_RADIUS_PROPERTY_NAME = "_Radius";
        private const float m_PORTAL_RADIUS_DISABLED_VALUE = 0.5f;


        private void Awake()
        {
            Initialize();
        }

        private void Initialize()
        {
            m_BattleModel = Dispatcher.GetModel<BattleModel>();

            m_StartBattleSequenceModel = Dispatcher.GetModel<StartBattleSequenceModel>();
            m_StartBattleSequenceModel.OnPlayerViewGraphicsInitialized += PlayerViewGraphicsInitializedHandler;

            Material mat = new Material(PortalRenderer.material);
            PortalRenderer.material = mat;
            PortalRenderer.enabled = false;

            PortalRenderer.material.SetFloat(m_PORTAL_RADIUS_PROPERTY_NAME, m_PORTAL_RADIUS_DISABLED_VALUE);

            PortalShowAnimator.PrewarmSequence();
            PortalHideAnimator.PrewarmSequence();
        }

        private void PlayerViewGraphicsInitializedHandler(Transform playerViewGraphics)
        {
            m_StartBattleSequenceModel.PlayerViewTransform.DOScale(0, 0f);
            m_StartBattleSequenceModel.PlayerViewTransform.position = FromPortalPos.position;

            StartAppearSequence();
        }

        private void StartAppearSequence()
        {
            PortalRenderer.enabled = true;
            VortexEffect.Play();

            VortexShowAnimator.PlaySequence();
            PortalShowAnimator.PlaySequence(PortalShowSequenceComplete);
        }

        private void PortalShowSequenceComplete()
        {
            for (int i = 0; i < SpawnEffects.Length; i++)
            {
                SpawnEffects[i].Play();
            }

            Vector3 targetPos = Dispatcher.GetModel<SpawnModel>().PlayerSpawnPosition;

            Sequence playerSequence = DOTween.Sequence();
            playerSequence.Append(m_StartBattleSequenceModel.PlayerViewTransform.DOScale(1, 0.2f));
            playerSequence.Append(m_StartBattleSequenceModel.PlayerViewTransform.DOMove(targetPos, 0.2f));
            playerSequence.AppendInterval(0.5f);
            playerSequence.AppendCallback(PlayerSequenceCompleteHandler);
            playerSequence.AppendInterval(0.5f);
            playerSequence.AppendCallback(MainSequenceCompletedHandler);
            playerSequence.AppendInterval(1f);
            playerSequence.AppendCallback(PortalHideSequenceComplete);
        }

        private void PlayerSequenceCompleteHandler()
        {
            VortexHideAnimator.PlaySequence();
            PortalHideAnimator.PlaySequence();
        }

        private void MainSequenceCompletedHandler()
        {
            m_StartBattleSequenceModel.OnMainSequenceCompleted?.Invoke();
        }

        private void PortalHideSequenceComplete()
        {
            m_StartBattleSequenceModel.OnSequenceFinished?.Invoke();

            PortalRenderer.enabled = false;
            Destroy(gameObject, 1);
        }
    }
}
