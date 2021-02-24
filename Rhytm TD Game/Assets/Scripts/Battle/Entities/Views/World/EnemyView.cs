using UnityEngine;

namespace RhytmTD.Battle.Entities.Views
{
    public class EnemyView : BattleEntityView
    {
        [SerializeField] private BattleEntityView[] ViewsToInit;

        private AnimationModule m_AnimationModule;
        private HealthModule m_HealthModule;

        public override void Initialize(BattleEntity entity)
        {
            base.Initialize(entity);

            m_AnimationModule = entity.GetModule<AnimationModule>();

            TransformModule transformModule = entity.GetModule<TransformModule>();
            transformModule.OnRotationChanged += RotationChanged;

            DestroyModule destroyModule = entity.GetModule<DestroyModule>();
            destroyModule.OnDestroyed += OnDestroyed;

            m_HealthModule = entity.GetModule<HealthModule>();
            m_HealthModule.OnHealthRemoved += OnHealthRemoved;

            foreach (BattleEntityView view in ViewsToInit)
            {
                view.Initialize(entity);
            }

            m_AnimationModule.PlayAnimation(CoreFramework.EnumsCollection.AnimationTypes.Show);
        }

        private void OnDestroyed(BattleEntity entity)
        {
            m_AnimationModule.PlayAnimation(CoreFramework.EnumsCollection.AnimationTypes.Destroy);
        }

        private void OnHealthRemoved(int health, int senderID)
        {
            if (m_HealthModule.CurrentHealth > 0)
            {
                m_AnimationModule.PlayAnimation(CoreFramework.EnumsCollection.AnimationTypes.TakeDamage);
            }
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
