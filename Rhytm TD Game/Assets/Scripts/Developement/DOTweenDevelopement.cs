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

        private Sequence m_HPSequence;

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                UseSequenceComponent();
            }

            if (Input.GetKeyDown(KeyCode.H))
            {
                HealthBar();
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                ReverseHealthBar();
            }
        }

        void ReverseHealthBar()
        {
            DOTweenSequenceAnimator animator = SequenceObject.GetComponent<DOTweenSequenceAnimator>();

            TweenContainer[] tweens = { new ShakeScaleTweenContainer(HealthBarTransform, ShakeSpeed, new Vector3(ShakeSpeed, 0, 0), ShakeVibrato, ShakeRandom, ShakeFade),
                                        new ScaleTweenContainer(HealthBarTransform, new Vector3(ShakeStrength, 0, 0), ScaleSpeed, false) };

            animator.PlaySequence(tweens);
        }

        void HealthBar()
        {
            DOTweenSequenceAnimator animator = SequenceObject.GetComponent<DOTweenSequenceAnimator>();

            TweenContainer[] tweens = { new ScaleTweenContainer(HealthBarTransform, new Vector3(ShakeStrength, 0, 0), ScaleSpeed, true),
                                        new ShakeScaleTweenContainer(HealthBarTransform, ShakeSpeed, new Vector3(ShakeSpeed, 0, 0), ShakeVibrato, ShakeRandom, ShakeFade) };

            animator.PlaySequence(tweens);
        }

        void UseSequenceComponent()
        {
            DOTweenSequenceAnimator animator = SequenceObject.GetComponent<DOTweenSequenceAnimator>();

            TweenContainer[] tweens = { new ScaleTweenContainer(EnemyAppearSimulationObject, Vector3.zero, ScaleSpeed, true),
                                        new ShakeScaleTweenContainer(EnemyAppearSimulationObject, ShakeSpeed, new Vector3(ShakeSpeed, ShakeSpeed, ShakeSpeed), ShakeVibrato, ShakeRandom, ShakeFade) };

            animator.PlaySequence(tweens);

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

