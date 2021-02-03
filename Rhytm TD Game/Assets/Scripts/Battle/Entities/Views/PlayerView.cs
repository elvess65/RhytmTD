
using RhytmTD.Battle.Entities.Controllers;
using UnityEngine;

namespace RhytmTD.Battle.Entities.Views
{
    public class PlayerView : BattleEntityView
    {
        // need to move this to somewhere :D
        private MoveController m_MoveController;

        public override void Initialize(BattleEntity entity)
        {
            base.Initialize(entity);

            m_MoveController = Dispatcher.GetController<MoveController>();

            entity.GetModule<MoveModule>().StartMove(Vector3.forward);
        }

        private void Update()
        {
            m_MoveController.MoveAll(Time.deltaTime);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, 1);
        }
    }
}
