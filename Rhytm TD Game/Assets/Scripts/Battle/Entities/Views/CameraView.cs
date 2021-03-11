using Cinemachine;
using CoreFramework;
using RhytmTD.Battle.Entities.Models;
using UnityEngine;
using static CoreFramework.EnumsCollection;

namespace RhytmTD.Battle.Entities.Views
{
    public class CameraView : BaseView
    {
        public Transform DefaultCameraPos;

        [Header("Cinemachine Virtual Cameras")]
        public CinemachineVirtualCamera VCDefault;
        public CinemachineVirtualCamera VCamMain;

        [Header("Cinemachine Additional")]
        public CinemachineBrain CMBrain;

        private CameraModel m_CameraModel;

        private void Awake()
        {
            Initialize();
        }

        private void Initialize()
        {
            m_CameraModel = Dispatcher.GetModel<CameraModel>();
            m_CameraModel.Cameras = new System.Collections.Generic.Dictionary<CameraTypes, CinemachineVirtualCameraBase>();            
            m_CameraModel.Cameras[CameraTypes.Default] = VCDefault;
            m_CameraModel.Cameras[CameraTypes.Main] = VCamMain;
            m_CameraModel.CurrentCamera = VCDefault;
            m_CameraModel.CMBrain = CMBrain;
            m_CameraModel.MainCamera = Camera.main;

            VCDefault.transform.position = DefaultCameraPos.position;
        }
    }
}
