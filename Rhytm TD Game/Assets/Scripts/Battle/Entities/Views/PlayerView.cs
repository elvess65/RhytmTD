using UnityEngine;

namespace RhytmTD.Battle.Entities.Views
{
    public class PlayerView : BattleEntityView
    {
        private MoveModule m_MoveModule;

        public override void Initialize(BattleEntity entity)
        {
            base.Initialize(entity);

            m_MoveModule = entity.GetModule<MoveModule>();
            m_MoveModule.OnPositionChanged += OnPositionChanged;
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
