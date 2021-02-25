using CoreFramework;
using System;
using static CoreFramework.EnumsCollection;

namespace RhytmTD.Battle.Entities.Models
{
    public class MarkerModel : BaseModel
    {
        public delegate void RadiusMarkerHandler(int markerID, BattleEntity battleEntity, float radius);
        public delegate void TargetMarkerHandler(int markerID, BattleEntity battleEntity);

        public event Action<int, MarkerTypes, BattleEntity> OnMarkerCreated;
        public event RadiusMarkerHandler OnRadiusMarkerShow;
        public event TargetMarkerHandler OnTargetMarkerShow;
        public event Action<int> OnMarkerHide;

        public void MarkerCreated(int markerID, MarkerTypes markerType, BattleEntity battleEntity)
        {
            OnMarkerCreated?.Invoke(markerID, markerType, battleEntity);
        }

        public void ShowRadiusAttackMarker(int markerID, BattleEntity battleEntity, float radius)
        {
            OnRadiusMarkerShow?.Invoke(markerID, battleEntity, radius);
        }

        public void ShowTargetMarker(int markerID, BattleEntity battleEntity)
        {
            OnTargetMarkerShow?.Invoke(markerID, battleEntity);
        }

        public void MarkerHide(int markerID)
        {
            OnMarkerHide?.Invoke(markerID);
        }
    }
}
