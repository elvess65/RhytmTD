using RhytmTD.Animation;
using RhytmTD.Animation.DOTween;
using UnityEngine;
using static CoreFramework.EnumsCollection;

namespace RhytmTD.Battle.Entities.Views
{
    public class EnemyEntityAnimationView : AbstractAnimationView
    {
        [Header("DOTween")]
        public Transform TweenControlledObject;  
        public float ScaleDuration = 0.2f;
        public float ShakeDuration = 0.2f;
        public float ShakeStrength = 0.5f;
        public int ShakeVibrato = 10;
        public int ShakeRandom = 90;
        public bool ShakeFade = false;

        private DOTweenSequenceAnimator DOTweenController;
        private TweenContainer[] m_ShowTweens;

        public override void Initialize()
        {
            base.Initialize();

            DOTweenController = GetComponent<DOTweenSequenceAnimator>();

            m_ShowTweens = new TweenContainer[] 
            {
                new ScaleTweenContainer(TweenControlledObject, Vector3.zero, ScaleDuration, true),
                new ShakeScaleTweenContainer(TweenControlledObject, ShakeDuration, new Vector3(ShakeDuration, ShakeDuration, ShakeDuration), ShakeVibrato, ShakeRandom, ShakeFade)
            };
        }

        public override void PlayAnimation(AnimationTypes animationType)
        {
            string key = GetKeyByType(animationType);

            switch (animationType)
            {
                case AnimationTypes.Attack:
                    SetTrigger(key);
                    break;

                case AnimationTypes.TakeDamage:
                    SetTrigger(key);
                    key = GetKeyByType(AnimationTypes.IdleBattle);
                    SetBool(key, true);
                    break;

                case AnimationTypes.Destroy:
                    SetTrigger(key);
                    break;

                case AnimationTypes.Show:
                    //SetTrigger(key);
                    DOTweenController.PlaySequence(m_ShowTweens);
                    break;

                case AnimationTypes.Victory:
                    SetTrigger(key);
                    break;

                case AnimationTypes.IdleBattle:
                    SetBool(key, true);
                    break;

                case AnimationTypes.IdleNormal:
                    key = GetKeyByType(AnimationTypes.IdleBattle);
                    SetBool(key, false);
                    break;
            }
        }
    }
}

