using UnityEngine;

namespace RhytmTD.Animation
{
    public class AnimationEventListener : MonoBehaviour
    {
        public System.Action OnAnimationMoment;

        public void MomentEventHandler()
        {
            OnAnimationMoment?.Invoke();
        }
    }
}
