using UnityEngine;

namespace RhytmTD.Assets.Abstract
{
    public abstract class AssetsManager<T> : MonoBehaviour where T: PrefabAssets
    {
        [SerializeField] private T m_PrefabAssets;

        public T GetAssets() => m_PrefabAssets;

        void Awake()
        {
            Initialize();
        }    

        protected virtual void Initialize()
        {
            m_PrefabAssets.Initialize();
        }
    }
}
