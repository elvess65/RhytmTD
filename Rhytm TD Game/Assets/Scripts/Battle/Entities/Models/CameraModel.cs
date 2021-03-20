using Cinemachine;
using CoreFramework;
using System.Collections.Generic;
using UnityEngine;

namespace RhytmTD.Battle.Entities.Models
{
    public class CameraModel : BaseModel
    {
        public int ActiveCamPriority = 10;

        public CinemachineVirtualCameraBase CurrentCamera;
        public CinemachineBrain CMBrain;
        public Camera MainCamera;

        public Dictionary<EnumsCollection.CameraTypes, CinemachineVirtualCameraBase> Cameras;
    }
}
