using CoreFramework;
using CoreFramework.Rhytm;
using RhytmTD.Battle.Entities.Models;
using RhytmTD.Data.Models;
using UnityEngine;

namespace RhytmTD.Battle.Entities.Controllers
{
    public class CameraFollowController : BaseController
    {
        private CameraModel m_CameraModel;
        private BattleModel m_BattleModel;
        private CameraController m_CameraController;
        private RhytmController m_RhytmController;
        private UpdateModel m_UpdateModel;

        private Vector3 m_CameraOffet;

        //FOV Pulse
        private bool m_IsActive = false;

        public CameraFollowController(Dispatcher dispatcher) : base(dispatcher)
        {
        }

        public override void InitializeComplete()
        {
            m_CameraModel = Dispatcher.GetModel<CameraModel>();
            m_BattleModel = Dispatcher.GetModel<BattleModel>();

            m_CameraController = Dispatcher.GetController<CameraController>();
            m_RhytmController = Dispatcher.GetController<RhytmController>();

            m_BattleModel.OnPlayerEntityInitialized += PlayerEntityInitialized;

            //FOV Pulse
            //m_RhytmController.OnTick += TickHandler;

            m_UpdateModel = Dispatcher.GetModel<UpdateModel>();
            m_UpdateModel.OnUpdate += Update;
        }

        //FOV Pulse
        private void TickHandler(int tick)
        {
            Camera.main.fieldOfView = 50;
            m_IsActive = true;
        }

        //FOV Pulse
        private void Update(float t)
        {
            if (m_IsActive && Camera.main.fieldOfView < 60)
            {
                Camera.main.fieldOfView += 5f * t;

                if (Camera.main.fieldOfView >= 60)
                {
                    Camera.main.fieldOfView = 60;
                    m_IsActive = false;
                }
            }
        }

        private void PlayerEntityInitialized(BattleEntity playerEntity)
        {
            if (playerEntity == null)
                return;

            TransformModule targetTransformModule = m_BattleModel.PlayerEntity.GetModule<TransformModule>();
            m_CameraOffet = m_CameraModel.Position - targetTransformModule.Position;

            targetTransformModule.OnPositionChanged += OnTargetPositionChanged;
        }

        private void OnTargetPositionChanged(Vector3 pos)
        {
            m_CameraController.SetCameraPosition(pos + m_CameraOffet);
        }
    }
}
