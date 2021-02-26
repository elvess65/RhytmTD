using System;
using static CoreFramework.EnumsCollection;

namespace RhytmTD.Battle.Entities
{
    /// <summary>
    /// Manages animations
    /// </summary>
    public class AnimationModule : IBattleModule
    {
        public event Action<AnimationTypes> OnPlayAnimation;
        public event Action<float> OnChangeSpeedMultiplayer;
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

        public void ChangeSpeedMultiplayer(float speedMultiplayer)
        {
            OnChangeSpeedMultiplayer?.Invoke(speedMultiplayer);
        }

        public void AnimationMomentHandler()
        {
            OnAnimationMoment?.Invoke();
        }
    }
}
