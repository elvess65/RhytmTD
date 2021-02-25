using Cinemachine;
using System.Collections.Generic;
using UnityEngine;
using static CoreFramework.EnumsCollection;

namespace RhytmTD.Developement
{
    public class CinemachineDevelopement : MonoBehaviour
    {
        public Transform Target;
        public CinemachineVirtualCamera Brain;
        public CinemachineVirtualCamera Default;
        public CinemachineVirtualCamera Main;

        private Dictionary<CameraTypes, CinemachineVirtualCameraBase> m_Cameras;

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.D))
            {
            }

            if (Input.GetKeyDown(KeyCode.M))
            {
               
            }
        }
    }
}

