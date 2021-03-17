
using UnityEngine;

namespace RhytmTD.Battle.Entities.Views
{
    public class MarkerView : BattleEntityView
    {
        private TransformModule m_TransformModule;

        public override void Initialize(BattleEntity entity)
        {
            base.Initialize(entity);

            m_TransformModule = entity.GetModule<TransformModule>();

            if (entity.HasModule<SyncPositionModule>())
            {
                m_TransformModule.OnPositionChanged += PositionChanged;
            }
        }

        private void PositionChanged(Vector3 position)
        {
            transform.position = position;
        }

        private void OnDestroy()
        {
            m_TransformModule.OnPositionChanged -= PositionChanged;
        }
    }
}
