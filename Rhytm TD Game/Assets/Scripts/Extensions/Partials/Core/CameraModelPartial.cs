using System.Collections.Generic;
using Cinemachine;

namespace CoreFramework
{
    public partial class CameraModel
    {
        public int ActiveCamPriority = 10;

        public CinemachineVirtualCameraBase CurrentCamera;
        public CinemachineBrain CMBrain;

        public Dictionary<EnumsCollection.CameraTypes, CinemachineVirtualCameraBase> Cameras;
    }
}
