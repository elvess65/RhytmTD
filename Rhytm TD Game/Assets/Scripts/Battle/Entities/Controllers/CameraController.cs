using CoreFramework;
using RhytmTD.Battle.Entities.Models;
using UnityEngine;

namespace RhytmTD.Battle.Entities.Controllers
{
    public class CameraController : BaseController
    {
        private CameraModel m_CameraModel;

        public CameraController(Dispatcher dispatcher) : base(dispatcher)
        {
        }

        public override void InitializeComplete()
        {
            m_CameraModel = Dispatcher.GetModel<CameraModel>();
        }

        public void SetCameraPosition(Vector3 pos)
        {
            m_CameraModel.Position = pos;
        }

        public void SetCamearRotatation(Quaternion rotation)
        {
            m_CameraModel.Rotation = rotation;
        }
    }
}
