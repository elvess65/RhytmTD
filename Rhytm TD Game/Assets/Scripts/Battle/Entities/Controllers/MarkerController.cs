using CoreFramework;
using RhytmTD.Battle.Entities.Models;
using System.Collections.Generic;

namespace RhytmTD.Battle.Entities.Controllers
{
    public class MarkerController : BaseController
    {
        private MarkerModel m_MarkerModel;

        // change to pool in the future
        private Dictionary<MarkerTypes, int> m_MarkerIdByType = new Dictionary<MarkerTypes, int>();
        private Dictionary<int, MarkerTypes> m_MarkerTypeById = new Dictionary<int, MarkerTypes>();

        public MarkerController(Dispatcher dispatcher) : base(dispatcher)
        {
        }

        public override void InitializeComplete()
        {
            m_MarkerModel = Dispatcher.GetModel<MarkerModel>();
        }

        public int ShowTargetMarker(MarkerTypes markerType, BattleEntity battleEntity)
        {
            int markerID = GetOrCreateMarker(markerType, battleEntity);
            m_MarkerModel.ShowTargetMarker(markerID, battleEntity);

            return markerID;
        }

        public int ShowAttackRadiusMarker(MarkerTypes markerType, BattleEntity battleEntity, float radius)
        {
            int markerID = GetOrCreateMarker(markerType, battleEntity);
            m_MarkerModel.ShowRadiusAttackMarker(markerID, battleEntity, radius);

            return markerID;
        }

        private int GetOrCreateMarker(MarkerTypes markerType, BattleEntity battleEntity)
        {
            if (m_MarkerIdByType.ContainsKey(markerType))
            {
                return m_MarkerIdByType[markerType];
            }
            else
            {
                int markerID = IDGenerator.GenerateID();

                m_MarkerModel.MarkerCreated(markerID, markerType, battleEntity);

                m_MarkerIdByType.Add(markerType, markerID);
                m_MarkerTypeById.Add(markerID, markerType);

                return markerID;
            }
        }

        public void HideMarker(int markerID)
        {
            MarkerTypes markerType = m_MarkerTypeById[markerID];

            m_MarkerModel.MarkerHide(markerID);

            m_MarkerIdByType.Remove(markerType);
            m_MarkerTypeById.Remove(markerID);
        }
    }
}
