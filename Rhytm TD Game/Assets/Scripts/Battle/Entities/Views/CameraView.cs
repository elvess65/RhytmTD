using CoreFramework;
using RhytmTD.Battle.Entities.Controllers;
using RhytmTD.Battle.Entities.Models;
using UnityEngine;

namespace RhytmTD.Battle.Entities.Views
{
    public class CameraView : BaseView
    {
        private CameraController m_CameraController;

        private void Awake()
        {
            CameraModel cameraModel = Dispatcher.GetModel<CameraModel>();
            cameraModel.OnPositionChanged += OnPositionChanged;
            cameraModel.OnRotationChanger += OnRotationChanged;

            m_CameraController = Dispatcher.GetController<CameraController>();
            m_CameraController.SetCameraPosition(transform.position);
            m_CameraController.SetCamearRotatation(transform.rotation);
        }

        private void OnPositionChanged(Vector3 pos)
        {
            transform.position = pos;
        }

        private void OnRotationChanged(Quaternion rot)
        {
            transform.rotation = rot;
        }
    }
}
