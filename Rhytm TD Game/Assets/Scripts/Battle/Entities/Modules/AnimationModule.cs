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

        public AnimationModule()
        {
        }

        public void PlayAnimation(AnimationTypes animationType)
        {
            OnPlayAnimation?.Invoke(animationType);
        }
    }
}
