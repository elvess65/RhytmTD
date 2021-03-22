using System;
using static CoreFramework.EnumsCollection;

namespace RhytmTD.Battle.Entities
{
    /// <summary>
    /// Manages animations
    /// </summary>
    public class AnimationModule : IBattleModule
    {
        public float AttackAnimationTime { get; set; }

        public event Action<AnimationTypes> OnPlayAnimation;
        public event Action OnAnimationMoment;

        /// <summary>
        /// Manages animations
        /// </summary>
        public AnimationModule()
        {
        }

        public void PlayAnimation(AnimationTypes animationType)
        {
            OnPlayAnimation?.Invoke(animationType);
        }

        public void AnimationMomentHandler()
        {
            OnAnimationMoment?.Invoke();
        }
    }
}
