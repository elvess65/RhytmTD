using UnityEngine;

namespace RhytmTD.Battle.Entities.Views
{
    public class EnemyView : BattleEntityView
    {
        [SerializeField] private BattleEntityView[] ViewsToInit;

        private TransformModule m_TransformModule;
        private DestroyModule m_DestroyModule;

        public override void Initialize(BattleEntity entity)
        {
            base.Initialize(entity);

            m_TransformModule = entity.GetModule<TransformModule>();
            m_TransformModule.OnRotationChanged += RotationChanged;

            m_DestroyModule = entity.GetModule<DestroyModule>();
            m_DestroyModule.OnDestroyed += OnDestroyed;

            foreach (BattleEntityView view in ViewsToInit)
            {
                view.Initialize(entity);
            }
        }

        private void OnDestroyed(BattleEntity entity)
        {
            Debug.Log($"VIEW: {entity.ID} was destroyed");

            transform.localScale = Vector3.one * 0.1f;
        }

        private void RotationChanged(Quaternion rotation)
        {
            transform.rotation = rotation;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, 1);
        }
    }
}
