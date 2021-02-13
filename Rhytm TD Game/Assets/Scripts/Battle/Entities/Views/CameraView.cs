using CoreFramework;
using RhytmTD.Battle.Entities.Controllers;
using RhytmTD.Battle.Entities.Models;
using System;
using UnityEngine;

namespace RhytmTD.Battle.Entities.Views
{
    public class CameraView : BaseView, IDisposable
    {
        private CameraModel m_CameraModel;
        private CameraController m_CameraController;

        private void Awake()
        {
            Initialize();
        }

        private void Initialize()
        {
            Dispatcher.AddDisposable(this);

            m_CameraModel = Dispatcher.GetModel<CameraModel>();
            m_CameraModel.OnPositionChanged += OnPositionChanged;
            m_CameraModel.OnRotationChanger += OnRotationChanged;

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

        public void Dispose()
        {
            m_CameraModel.OnPositionChanged -= OnPositionChanged;
            m_CameraModel.OnRotationChanger -= OnRotationChanged;

            Dispatcher.RemoveDisposable(this);
        }
    }
}
