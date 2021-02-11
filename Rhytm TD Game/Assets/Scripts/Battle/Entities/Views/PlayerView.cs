using UnityEngine;

namespace RhytmTD.Battle.Entities.Views
{
    public class PlayerView : BattleEntityView
    {
        private TransformModule m_TransformModule;

        public override void Initialize(BattleEntity entity)
        {
            base.Initialize(entity);

            m_TransformModule = entity.GetModule<TransformModule>();
            m_TransformModule.OnPositionChanged += OnPositionChanged;
        }

        private void OnPositionChanged(Vector3 position)
        {
            transform.position = position;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, 1);
        }
    }
}
