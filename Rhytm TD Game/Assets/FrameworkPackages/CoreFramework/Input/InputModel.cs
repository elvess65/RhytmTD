using UnityEngine;

namespace CoreFramework.Input
{
    public class InputModel : BaseModel
    {
        public event System.Action<Vector3> OnTouch;
        public event System.Action<KeyCode> OnKeyDown;

        public Vector3 LastTouchHitPoint;

        public void Touch(Vector3 position)
        {
            OnTouch?.Invoke(position);
        }

        public void KeyDown(KeyCode keyCode)
        {
            OnKeyDown?.Invoke(keyCode);
        }
    }
}
