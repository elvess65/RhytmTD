using RhytmTD.Animation.DOTween;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace RhytmTD.Developement
{
    public class AppearPortal : MonoBehaviour
    {
        public MeshRenderer PortalRenderer;
        public DOTweenSequenceAnimator GraphicsAnimator;
        public DOTweenSequenceAnimator GraphicsAnimato2;
        public ParticleSystem[] AppearEffects;
        public GameObject Obj;
        public Transform BeforePos;

        private Vector3 startPos; 

        private void Start()
        {
            Material mat = new Material(PortalRenderer.material);
            PortalRenderer.material = mat;

            PortalRenderer.material.DOFloat(0.5f, "_Radius", 0f);

            startPos = Obj.transform.position;
            Obj.gameObject.transform.position = BeforePos.position;
            Obj.gameObject.SetActive(false);
            GraphicsAnimator.gameObject.SetActive(false);
            PortalRenderer.gameObject.SetActive(false);
            for (int i = 0; i < AppearEffects.Length; i++)
            {
                AppearEffects[i].gameObject.SetActive(false);
            }
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                GraphicsAnimator.gameObject.SetActive(true);
                PortalRenderer.gameObject.SetActive(true);

                GraphicsAnimator.PlaySequence();
                Sequence sequence = DG.Tweening.DOTween.Sequence();
                sequence.AppendInterval(0.2f);
                sequence.Append(PortalRenderer.material.DOFloat(18, "_Radius", 1f).SetEase(Ease.OutCubic));
                sequence.Append(PortalRenderer.material.DOFloat(15, "_Radius", 0.5f).SetEase(Ease.InQuad));
                sequence.AppendCallback(CompleteShow);

            }

            if (Input.GetKeyDown(KeyCode.H))
            {
              
            }
        }

        void CompleteShow()
        {
            for (int i =0; i < AppearEffects.Length; i++)
            {
                AppearEffects[i].gameObject.SetActive(true);
            }

            Obj.gameObject.SetActive(true);
            Obj.transform.DOScale(0, 0.2f).From();
            Obj.transform.DOMove(startPos, 0.2f);

            GraphicsAnimato2.PlaySequence(Complete);
            Sequence sequence = DG.Tweening.DOTween.Sequence();
            sequence.AppendInterval(1f);
            sequence.Append(PortalRenderer.material.DOFloat(18, "_Radius", 0.2f));
            sequence.Append(PortalRenderer.material.DOFloat(0.5f, "_Radius", 1f));
            sequence.AppendCallback(Complete);
        }

        void Complete()
        {
            GraphicsAnimator.gameObject.SetActive(false);
            PortalRenderer.gameObject.SetActive(false);
        }
    }
}
