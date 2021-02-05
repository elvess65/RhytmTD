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
            m_MoveModule.OnStartMove += OnStartMove;
            m_MoveModule.OnStopMove += OnStopMove;
        }

        private void OnStartMove()
        {
            Debug.Log($"VIEW: Start move");
        }

        private void OnStopMove()
        {
            Debug.Log($"VIEW: stop move");
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, 1);
        }
    }
}
