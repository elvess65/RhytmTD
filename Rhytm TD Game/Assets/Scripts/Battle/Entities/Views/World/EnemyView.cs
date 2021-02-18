using UnityEngine;

namespace RhytmTD.Battle.Entities.Views
{
    [RequireComponent(typeof(EnemyEntityAnimationView))]
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
            Destroy(gameObject);
        }

        private void RotationChanged(Quaternion rotation)
        {
            transform.rotation = rotation;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position + new Vector3(0, 1, 0), 1);
        }
    }
}
