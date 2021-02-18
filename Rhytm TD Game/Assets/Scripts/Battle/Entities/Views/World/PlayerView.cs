using RhytmTD.Animation;
using UnityEngine;
using static CoreFramework.EnumsCollection;

namespace RhytmTD.Battle.Entities.Views
{
    public class PlayerView : BattleEntityView
    {
        private AbstractAnimationView m_AnimationView;

        public override void Initialize(BattleEntity entity)
        {
            base.Initialize(entity);

            m_AnimationView = GetComponent<AbstractAnimationView>();
            m_AnimationView.Initialize();

            SlotModule slotModule = entity.GetModule<SlotModule>();
            BattleEntitySlotView slotView = GetComponent<BattleEntitySlotView>();
            slotModule.Initialize(slotView.ProjectileSlot, slotView.HitSlot);

            MoveModule moveModule = entity.GetModule<MoveModule>();
            moveModule.OnMoveStarted += OnMoveStarted;
            moveModule.OnMoveStopped += OnMoveStopped;

            TransformModule transformModule = entity.GetModule<TransformModule>();
            transformModule.OnPositionChanged += OnPositionChanged;

            HealthModule healthModule = entity.GetModule<HealthModule>();
            healthModule.OnHealthRemoved += OnHealthRemoved;

            DestroyModule destroyModule = entity.GetModule<DestroyModule>();
            destroyModule.OnDestroyed += OnDestroyed;
        }

        private void OnMoveStarted()
        {
            m_AnimationView.PlayAnimation(AnimationTypes.StartMove);
        }

        private void OnMoveStopped()
        {
            m_AnimationView.PlayAnimation(AnimationTypes.StopMove);
        }

        private void OnHealthRemoved(int health, int senderID)
        {
            m_AnimationView.PlayAnimation(AnimationTypes.TakeDamage);
        }

        private void OnDestroyed(BattleEntity entity)
        {
            m_AnimationView.PlayAnimation(AnimationTypes.Destroy);
        }

        private void OnPositionChanged(Vector3 position)
        {
            transform.position = position;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position + new Vector3(0, 1.5f, 0), 2);
        }
    }
}
