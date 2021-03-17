using CoreFramework;
using RhytmTD.Battle.Entities.Models;
using System.Collections.Generic;
using UnityEngine;
using static CoreFramework.EnumsCollection;

namespace RhytmTD.Battle.Entities.Views
{
    public class MarkerFactoryView : BaseView
    {
        public GameObject[] MarkerPrefabs;

        private MarkerModel m_MarkerModel;

        private Dictionary<int, GameObject> m_Markers = new Dictionary<int, GameObject>();

        private void Awake()
        {
            Initialize();
        }

        private void Initialize()
        {
            m_MarkerModel = Dispatcher.GetModel<MarkerModel>();

            m_MarkerModel.OnMarkerShow += MarkerShow;
            m_MarkerModel.OnRadiusMarkerShow += RadiusMarkerShowed;
            m_MarkerModel.OnFollowingMarkerShow += FollowingMarkerShow;
            m_MarkerModel.OnMarkerHide += MarkerHide;
        }

        private void MarkerShow(MarkerTypes markerType, BattleEntity battleEntity) 
        {
            TransformModule transformModule = battleEntity.GetModule<TransformModule>();
            CreateMarker(markerType, battleEntity, transformModule.Position);
        }

        private void RadiusMarkerShowed(MarkerTypes markerType, BattleEntity battleEntity, float radius)
        {
            TransformModule transformModule = battleEntity.GetModule<TransformModule>();
            GameObject markerGo = CreateMarker(markerType, battleEntity, transformModule.Position);

            markerGo.transform.position = transformModule.Position;
            markerGo.transform.localScale = new Vector3(radius + radius, markerGo.transform.localScale.y, radius + radius);
        }

        private void FollowingMarkerShow(MarkerTypes markerType, BattleEntity battleEntity)
        {
            TransformModule transformModule = battleEntity.GetModule<TransformModule>();
            CreateMarker(markerType, battleEntity, transformModule.Position);
        }

        private GameObject CreateMarker(MarkerTypes markerType, BattleEntity battleEntity, Vector3 position)
        {
            GameObject go = Instantiate(MarkerPrefabs[(int)markerType], position, Quaternion.identity);
            m_Markers.Add(battleEntity.ID, go);

            BattleEntityView view = go.GetComponent<BattleEntityView>();
            view.Initialize(battleEntity);

            return go;
        }

        private void MarkerHide(int markerID)
        {
            GameObject go = m_Markers[markerID];
            Destroy(go);

            m_Markers.Remove(markerID);
        }

        private void OnDestroy()
        {
            m_MarkerModel.OnMarkerShow -= MarkerShow;
            m_MarkerModel.OnRadiusMarkerShow -= RadiusMarkerShowed;
            m_MarkerModel.OnFollowingMarkerShow -= FollowingMarkerShow;
            m_MarkerModel.OnMarkerHide -= MarkerHide;
        }
    }
}
