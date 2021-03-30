//#define LOG

using CoreFramework;
using RhytmTD.Battle.Entities.Models;
using UnityEngine;

namespace RhytmTD.Battle.Entities.Controllers
{
    public class EnviromentController : BaseController
    {
        private BattleModel m_BattleModel;
        private UpdateModel m_UpdateModel;
        private EnviromentModel m_EnviromentModel;

        private TransformModule m_PlayerTransformModule;

        private const int m_DEFAULT_AMOUNT_OF_CELLS = 2;


        public EnviromentController(Dispatcher dispatcher) : base(dispatcher)
        {
        }

        public override void InitializeComplete()
        {
            base.InitializeComplete();

            m_BattleModel = Dispatcher.GetModel<BattleModel>();
            m_UpdateModel = Dispatcher.GetModel<UpdateModel>();
            m_EnviromentModel = Dispatcher.GetModel<EnviromentModel>();

            m_BattleModel.OnBattleInitialize += BattleInitializeHandler;
            m_BattleModel.OnBattleStarted += BattleStartedHandler;
            m_BattleModel.OnBattleFinished += BattleFinishedHandler;
            m_BattleModel.OnPlayerEntityInitialized += PlayerInitializedHandler;
        }

        private void UpdateHandler(float deltaTime)
        {
            float sqrDistToLastEdge = (m_EnviromentModel.LastCellFarEdgePos - m_PlayerTransformModule.Position).sqrMagnitude;
            float sqrDistToFirstEdge = (m_EnviromentModel.FirstCellNearEdgePos - m_PlayerTransformModule.Position).sqrMagnitude;

#if UNITY_EDITOR
            Vector3 offset = Vector3.up * 2;
            Debug.DrawLine(m_PlayerTransformModule.Position + offset, m_EnviromentModel.LastCellFarEdgePos + offset, Color.green);
            Debug.DrawLine(m_PlayerTransformModule.Position + offset, m_EnviromentModel.FirstCellNearEdgePos + offset, Color.red);
#endif

#if LOG

            Debug.Log("Create: (" + sqrDistToLastEdge + " : " + m_EnviromentModel.SqrDistanceToSpawn +
                      ") Remove: (" + (sqrDistToFirstEdge + " : " + m_EnviromentModel.SqrDistanceToRemove));
#endif

            if (sqrDistToLastEdge <= m_EnviromentModel.SqrDistanceToSpawn)
            {
                m_EnviromentModel.OnCellShouldBeAdded?.Invoke();
            }

            if (sqrDistToFirstEdge >= m_EnviromentModel.SqrDistanceToRemove)
            {
                m_EnviromentModel.RemoveCell();
            }
        }


        #region Handlers

        private void BattleInitializeHandler()
        {
            m_EnviromentModel.OnInitializeEnviroment?.Invoke();

            for (int i = 0; i < m_DEFAULT_AMOUNT_OF_CELLS; i++)
            {
                m_EnviromentModel.OnCellShouldBeAdded?.Invoke();
            }
        }

        private void PlayerInitializedHandler(BattleEntity entity)
        {
            m_PlayerTransformModule = entity.GetModule<TransformModule>();
        }

        private void BattleStartedHandler()
        {
            m_UpdateModel.OnUpdate += UpdateHandler;
        }

        private void BattleFinishedHandler(bool isSuccess)
        {
            m_UpdateModel.OnUpdate -= UpdateHandler;
        }

        #endregion
    }
}
