using CoreFramework;
using UnityEngine;

namespace RhytmTD.Battle.Entities.Views
{
    public class ProjectileView : BaseView
    {
        private TransformModule m_TransformModule;
        private DestroyModule m_DestroyModule;

        public void Initialize(BattleEntity entity)
        {
            m_TransformModule = entity.GetModule<TransformModule>();
            m_TransformModule.OnPositionChanged += OnPositionChanged;

            m_DestroyModule = entity.GetModule<DestroyModule>();
            m_DestroyModule.OnDestroyed += OnDestroyed;
        }

        private void OnPositionChanged(Vector3 position)
        {
            transform.position = position;
        }

        private void OnDestroyed(BattleEntity entity)
        {
            Destroy(gameObject);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, 1);
        }
    }
}
