using UnityEngine;

namespace RhytmTD.Battle.Entities.Views
{
    public class EnemyView : BattleEntityView
    {
        private TransformModule m_TransformModule;
        private FocusModule m_FocusModule;

        public override void Initialize(BattleEntity entity)
        {
            base.Initialize(entity);

            m_TransformModule = entity.GetModule<TransformModule>();
            m_TransformModule.OnRotationChanged += RotationChanged;

            m_FocusModule = entity.GetModule<FocusModule>();
            m_FocusModule.OnFocusTargetChanged += OnFocusTargetChanged;
        }

        private void RotationChanged(Quaternion rotation)
        {
            transform.rotation = rotation;
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
