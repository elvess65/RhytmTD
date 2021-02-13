using UnityEngine;

namespace RhytmTD.Battle.Entities.Views
{
    public class PlayerView : BattleEntityView
    {
        private TransformModule m_TransformModule;
        private DestroyModule m_DestroyModule;

        public override void Initialize(BattleEntity entity)
        {
            base.Initialize(entity);

            m_TransformModule = entity.GetModule<TransformModule>();
            m_TransformModule.OnPositionChanged += OnPositionChanged;

            m_DestroyModule = entity.GetModule<DestroyModule>();
            m_DestroyModule.OnDestroyed += OnDestroyed;
        }

        private void OnDestroyed(BattleEntity entity)
        {
            Debug.Log($"VIEW: {entity.ID} was destroyed");

            transform.localScale = Vector3.one * 0.1f;
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
