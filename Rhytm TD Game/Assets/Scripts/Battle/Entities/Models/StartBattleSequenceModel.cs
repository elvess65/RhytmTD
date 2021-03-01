using CoreFramework;
using UnityEngine;

namespace RhytmTD.Battle.Entities.Models
{
    public class StartBattleSequenceModel : BaseModel
    {
        private Transform m_PlayerViewGraphics;

        public Transform PlayerViewTransform
        {
            get => m_PlayerViewGraphics;
            set
            {
                m_PlayerViewGraphics = value;
                OnPlayerViewGraphicsInitialized?.Invoke(m_PlayerViewGraphics);
            }
        }

        public System.Action<Transform> OnPlayerViewGraphicsInitialized;
        public System.Action OnMainSequenceCompleted;
        public System.Action OnSequenceFinished;
    }
}
