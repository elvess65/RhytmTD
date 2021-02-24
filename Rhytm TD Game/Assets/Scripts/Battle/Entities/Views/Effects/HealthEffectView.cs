using UnityEngine;

namespace RhytmTD.Battle.Entities.Views.Effects
{
    public class HealthEffectView : BattleEntityView
    {
        private TransformModule m_TransformModule;

        public override void Initialize(BattleEntity entity)
        {
            base.Initialize(entity);

            m_TransformModule = entity.GetModule<TransformModule>();
            m_TransformModule.OnPositionChanged += PositionChanged;

            transform.position = m_TransformModule.Position;
        }

        private void PositionChanged(Vector3 position)
        {
            transform.position = position;
        }
    }
}
