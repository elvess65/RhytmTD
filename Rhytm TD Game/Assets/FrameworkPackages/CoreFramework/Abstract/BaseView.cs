
using UnityEngine;

namespace CoreFramework
{
    public abstract class BaseView : MonoBehaviour
    {
        public Dispatcher Dispatcher => Dispatcher.Instance;
    }
}
