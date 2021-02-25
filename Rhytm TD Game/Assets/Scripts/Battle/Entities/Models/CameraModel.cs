using Cinemachine;
using CoreFramework;
using System.Collections.Generic;

namespace RhytmTD.Battle.Entities.Models
{
    public class CameraModel : BaseModel
    {
        public int ActiveCamPriority = 10;

        public CinemachineVirtualCameraBase CurrentCamera;
        public CinemachineBrain CMBrain;

        public Dictionary<EnumsCollection.CameraTypes, CinemachineVirtualCameraBase> Cameras;
    }
}
