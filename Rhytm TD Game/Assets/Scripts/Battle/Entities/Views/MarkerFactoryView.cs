using CoreFramework;
using RhytmTD.Battle.Entities.Models;
using System.Collections.Generic;
using UnityEngine;

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

            m_MarkerModel.OnMarkerCreated += MarkerCreated;
            m_MarkerModel.OnRadiusMarkerShow += RadiusMarkerShow;
            m_MarkerModel.OnTargetMarkerShow += TargetMarkerShow;
            m_MarkerModel.OnMarkerHide += MarkerHide;
        }

        private void MarkerCreated(int markerID, MarkerTypes markerType, BattleEntity battleEntity)
        {
            TransformModule transformModule = battleEntity.GetModule<TransformModule>();

            GameObject go = Instantiate(MarkerPrefabs[(int)markerType], transformModule.Position, Quaternion.identity);
            m_Markers.Add(markerID, go);
        }

        private void RadiusMarkerShow(int markerID, BattleEntity battleEntity, float radius)
        {
            TransformModule transformModule = battleEntity.GetModule<TransformModule>();
            GameObject markerGo = m_Markers[markerID];

            markerGo.transform.position = transformModule.Position;
            markerGo.transform.localScale = new Vector3(radius + radius, markerGo.transform.localScale.y, radius + radius);
        }

        private void TargetMarkerShow(int markerID, BattleEntity battleEntity)
        {
            TransformModule transformModule = battleEntity.GetModule<TransformModule>();
            GameObject markerGo = m_Markers[markerID];

            markerGo.transform.position = transformModule.Position;
        }

        private void MarkerHide(int markerID)
        {
            GameObject go = m_Markers[markerID];
            Destroy(go);
        }

        private void OnDestroy()
        {
            m_MarkerModel.OnMarkerCreated -= MarkerCreated;
            m_MarkerModel.OnRadiusMarkerShow -= RadiusMarkerShow;
            m_MarkerModel.OnTargetMarkerShow -= TargetMarkerShow;
            m_MarkerModel.OnMarkerHide -= MarkerHide;
        }
    }
}
