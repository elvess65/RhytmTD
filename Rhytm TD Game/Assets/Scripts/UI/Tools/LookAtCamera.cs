using RhytmTD.Battle.Core;
using UnityEngine;

namespace RhytmTD.UI.Tools
{
    public class LookAtCamera : MonoBehaviour
    {
        private Camera m_Camera;

        void Start()
        {
            //m_Camera = BattleManager.Instance.CamerasHolder.WorldCamera;
        }

        void Update()
        {
            transform.LookAt(m_Camera.transform);
        }
    }
}
