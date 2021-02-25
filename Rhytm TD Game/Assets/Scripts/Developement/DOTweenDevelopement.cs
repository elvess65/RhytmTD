using UnityEngine;
using DG.Tweening;
using RhytmTD.Animation.DOTween;

namespace RhytmTD.Developement
{
    public class DOTweenDevelopement : MonoBehaviour
    {
        public RectTransform HealthBarTransform;
        public GameObject SequenceObject;
        public Transform EnemyAppearSimulationObject;
        public float ScaleSpeed = 0.2f;
        public float ShakeSpeed = 0.2f;
        public float ShakeStrength = 0.5f;
        public int ShakeVibrato = 10;
        public int ShakeRandom = 90;
        public bool ShakeFade = false;
        public DOTweenSequenceAnimator Animator;
        public DOTweenSequenceAnimator HideAnimator;

        private Sequence m_HPSequence;

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
            }

            if (Input.GetKeyDown(KeyCode.H))
            {
                m_HPSequence = Animator.PlayExposedSequence(() => Debug.Log("Finish appear"));
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                HideAnimator.PlayExposedSequence(() => Debug.Log("Finish hide"));
            }
        }

        void ReverseHealthBar()
        {

        }

        void EnemyAppearSimulation()
        {
            Sequence sequence = DOTween.Sequence();
            sequence.OnComplete(OnEnemyAppearSimulationComplete);

            sequence.Append(EnemyAppearSimulationObject.DOScale(0, ScaleSpeed).From());
            sequence.Append(EnemyAppearSimulationObject.DOShakeScale(ShakeSpeed, ShakeSpeed, ShakeVibrato, ShakeRandom, ShakeFade));
        }

        void OnEnemyAppearSimulationComplete()
        {
            Debug.Log("OnEnemyAppearSimulationComplete");
        }
    }
}

