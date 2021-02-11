using UnityEngine;

namespace RhytmTD.Battle.Entities.Views
{
    public class EnemyView : BattleEntityView
    {
        private FocusModule m_TransformModule;

        public override void Initialize(BattleEntity entity)
        {
            base.Initialize(entity);

            m_TransformModule = entity.GetModule<FocusModule>();
            m_TransformModule.OnFocusTargetChanged += OnFocusTargetChanged;
        }

        private void OnFocusTargetChanged(int targetID)
        {
            Debug.Log($"VIEW: OnFocusTargetChanged " + targetID);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, 1);
        }
    }
}
