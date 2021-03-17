using CoreFramework;
using RhytmTD.Battle.Entities.Models;
using System.Collections.Generic;
using UnityEngine;
using static CoreFramework.EnumsCollection;

namespace RhytmTD.Battle.Entities.Controllers
{
    public class MarkerController : BaseController
    {
        private MarkerModel m_MarkerModel;
        private BattleModel m_BattleModel;

        // change to pool in the future
        private Dictionary<int, BattleEntity> m_MarkerById = new Dictionary<int, BattleEntity>();

        public MarkerController(Dispatcher dispatcher) : base(dispatcher)
        {
        }

        public override void InitializeComplete()
        {
            m_MarkerModel = Dispatcher.GetModel<MarkerModel>();
            m_BattleModel = Dispatcher.GetModel<BattleModel>();
        }

        private BattleEntity CreateMarkerAtPosition(Vector3 postion)
        {
            BattleEntity markerEntity = new BattleEntity(IDGenerator.GenerateID());
            markerEntity.AddModule(new TransformModule(postion, Quaternion.identity));

            m_MarkerById.Add(markerEntity.ID, markerEntity);

            return markerEntity;
        } 

        public int ShowMarkerAtPosition(MarkerTypes markerType, Vector3 postion)
        {
            BattleEntity markerEntity = CreateMarkerAtPosition(postion);

            m_MarkerModel.MarkerShowed(markerType, markerEntity);

            return markerEntity.ID;
        }

        public int ShowRadiusMarkerAtPosition(MarkerTypes markerType, Vector3 postion, float radius)
        {
            BattleEntity markerEntity = CreateMarkerAtPosition(postion);

            m_MarkerModel.RadiusMarkerShowed(markerType, markerEntity, radius);

            return markerEntity.ID;
        }

        public int ShowTargetFollowingMarker(MarkerTypes markerType, BattleEntity target)
        {
            TransformModule targetTransformModule = target.GetModule<TransformModule>();

            BattleEntity markerEntity = CreateMarkerAtPosition(targetTransformModule.Position);
            markerEntity.AddModule(new SyncPositionModule(target));

            m_MarkerModel.MarkerShowed(markerType, markerEntity);

            m_BattleModel.AddBattleEntity(markerEntity);

            return markerEntity.ID;
        }

        public void HideMarker(int markerID)
        {
            if (!m_MarkerById.ContainsKey(markerID))
                return;

            m_MarkerModel.MarkerHided(markerID);

            m_MarkerById.Remove(markerID);

            if (m_BattleModel.HasEntity(markerID))
            {
                m_BattleModel.RemoveBattleEntity(markerID);
            }
        }
    }
}
