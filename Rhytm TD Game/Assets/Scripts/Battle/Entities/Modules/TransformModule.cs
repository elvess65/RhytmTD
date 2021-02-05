using UnityEngine;

namespace RhytmTD.Battle.Entities
{
    /// <summary>
    /// Позиция и фокусировка
    /// </summary>
    public class TransformModule : IBattleModule
    {
        public Transform Transform { get; private set; }
        public Transform FocusTarget { get; private set; }
        public float Speed { get; private set; }
        public float CurrentSpeed { get; private set; }
        public bool IsFocusing { get; private set; }

        public System.Action OnStartFocus;
        public System.Action OnStopFocus;

        public TransformModule(Transform transform, float speed)
        {
            Transform = transform;
            Speed = CurrentSpeed = speed;
        }

        public void StartFocus(TransformModule targetTransformModule)
        {
            if (IsFocusing)
                return;

            FocusTarget = targetTransformModule.Transform;
            IsFocusing = true;

            OnStartFocus?.Invoke();
        }

        public void StopFocus()
        {
            if (!IsFocusing)
                return;

            FocusTarget = null;
            IsFocusing = false;

            OnStopFocus?.Invoke();
        }
    }
}
