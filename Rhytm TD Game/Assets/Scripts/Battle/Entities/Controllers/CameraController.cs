using CoreFramework;
using RhytmTD.Battle.Entities.Models;
using UnityEngine;

namespace RhytmTD.Battle.Entities.Controllers
{
    public class CameraController : BaseController
    {
        private CameraModel m_CameraModel;
        private BattleModel m_BattleModel;

        private StartBattleSequenceModel m_StartBattleSequenceModel;

        public CameraController(Dispatcher dispatcher) : base(dispatcher)
        {
        }

        public override void InitializeComplete()
        {
            m_CameraModel = Dispatcher.GetModel<CameraModel>();
            m_BattleModel = Dispatcher.GetModel<BattleModel>();
            m_BattleModel.OnPlayerEntityInitialized += PlayerInitializedHandler;

            m_StartBattleSequenceModel = Dispatcher.GetModel<StartBattleSequenceModel>();
            m_StartBattleSequenceModel.OnSequencePrepared += SequencePreparedHandler;
        }

        public void SetTarget(Transform transform)
        {
            m_CameraModel.Cameras[EnumsCollection.CameraTypes.Default].LookAt = transform;
            m_CameraModel.Cameras[EnumsCollection.CameraTypes.Main].Follow = transform;
        }

        private void ActivateCamera(EnumsCollection.CameraTypes cameraType)
        {
            m_CameraModel.CurrentCamera.Priority = 0;
            m_CameraModel.CurrentCamera = m_CameraModel.Cameras[cameraType];
            m_CameraModel.CurrentCamera.Priority = m_CameraModel.ActiveCamPriority;
        }

        private void SequencePreparedHandler()
        {
            ActivateCamera(EnumsCollection.CameraTypes.Main);
        }

        private void PlayerInitializedHandler(BattleEntity playerBattleEntity)
        {
            ActivateCamera(EnumsCollection.CameraTypes.Default);
        }
    }
}
