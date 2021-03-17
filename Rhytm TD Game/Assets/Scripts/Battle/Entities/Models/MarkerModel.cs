using CoreFramework;
using System;
using static CoreFramework.EnumsCollection;

namespace RhytmTD.Battle.Entities.Models
{
    public class MarkerModel : BaseModel
    {
        public delegate void RadiusMarkerHandler(MarkerTypes markerType, BattleEntity battleEntity, float radius);
        public delegate void TargetMarkerHandler(MarkerTypes markerType, BattleEntity battleEntity);

        public event TargetMarkerHandler OnMarkerShow;
        public event RadiusMarkerHandler OnRadiusMarkerShow;
        public event TargetMarkerHandler OnFollowingMarkerShow;
        public event Action<int> OnMarkerHide;

        public void MarkerShowed(MarkerTypes markerType, BattleEntity battleEntity)
        {
            OnMarkerShow?.Invoke(markerType, battleEntity);
        }

        public void RadiusMarkerShowed(MarkerTypes markerType, BattleEntity battleEntity, float radius)
        {
            OnRadiusMarkerShow?.Invoke(markerType, battleEntity, radius);
        }

        public void FollowingMarkerShowed(MarkerTypes markerType, BattleEntity battleEntity, BattleEntity target)
        {
            OnFollowingMarkerShow?.Invoke(markerType, battleEntity);
        }

        public void MarkerHided(int markerID)
        {
            OnMarkerHide?.Invoke(markerID);
        }
    }
}
