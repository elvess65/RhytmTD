using CoreFramework;
using RhytmTD.Battle.Entities.Models;
using UnityEngine;

namespace RhytmTD.Battle.Entities.Controllers
{
    public class CameraFollowController : BaseController
    {
        private CameraModel m_CameraModel;
        private BattleModel m_BattleModel;
        private CameraController m_CameraController;

        private Vector3 m_CameraOffet;

        public CameraFollowController(Dispatcher dispatcher) : base(dispatcher)
        {
        }

        public override void InitializeComplete()
        {
            m_CameraModel = Dispatcher.GetModel<CameraModel>();
            m_BattleModel = Dispatcher.GetModel<BattleModel>();

            m_CameraController = Dispatcher.GetController<CameraController>();

            m_BattleModel.OnPlayerEntityInitialized += PlayerEntityInitialized;
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
