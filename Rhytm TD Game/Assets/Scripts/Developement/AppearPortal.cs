using FrameworkPackages.DOTween;
using UnityEngine;
using DG.Tweening;

namespace RhytmTD.Developement
{
    public class AppearPortal : MonoBehaviour
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
        public Transform ToPortalPos;

        public GameObject Player;

        private void Start()
        {
            Material mat = new Material(PortalRenderer.material);
            PortalRenderer.material = mat;
            PortalRenderer.enabled = false;

            PortalRenderer.material.SetFloat("_Radius", 0.5f);

            PortalShowAnimator.PrewarmSequence();
            PortalHideAnimator.PrewarmSequence();

            Player.transform.position = FromPortalPos.position;
            Player.gameObject.SetActive(false);
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                PortalRenderer.enabled = true;
                VortexEffect.Play();

                VortexShowAnimator.PlaySequence();
                PortalShowAnimator.PlaySequence(PortalShowSequenceComplete);
            }
        }

        void PortalShowSequenceComplete()
        {
            Player.gameObject.SetActive(true);

            for (int i =0; i < SpawnEffects.Length; i++)
            {
                SpawnEffects[i].Play();
            }

            Sequence playerSequence = DOTween.Sequence();
            playerSequence.Append(Player.transform.DOScale(0, 0.2f).From());
            playerSequence.Append(Player.transform.DOMove(ToPortalPos.position, 0.2f));
            playerSequence.AppendCallback(PlayerSequenceCompleteHandler);
        }

        void PlayerSequenceCompleteHandler()
        {
            VortexHideAnimator.PlaySequence(PortalHideSequenceComplete);
            PortalHideAnimator.PlaySequence();
        }

        void PortalHideSequenceComplete()
        {
            PortalRenderer.enabled = false;
        }
    }
}
