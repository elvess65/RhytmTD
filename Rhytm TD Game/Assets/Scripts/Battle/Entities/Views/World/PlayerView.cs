﻿using UnityEngine;
using static CoreFramework.EnumsCollection;

namespace RhytmTD.Battle.Entities.Views
{
    public class PlayerView : BattleEntityView
    {
        private AnimationModule m_AnimationModule;

        public override void Initialize(BattleEntity entity)
        {
            base.Initialize(entity);

            m_AnimationModule = entity.GetModule<AnimationModule>();
            m_AnimationModule.InitializeModule(this);

            SlotModule slotModule = entity.GetModule<SlotModule>();
            slotModule.InitializeModule(this);
            
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
            m_AnimationModule.PlayAnimation(AnimationTypes.StartMove);
        }

        private void OnMoveStopped()
        {
            m_AnimationModule.PlayAnimation(AnimationTypes.StopMove);
        }

        private void OnHealthRemoved(int health, int senderID)
        {
            m_AnimationModule.PlayAnimation(AnimationTypes.TakeDamage);
        }

        private void OnDestroyed(BattleEntity entity)
        {
            m_AnimationModule.PlayAnimation(AnimationTypes.Destroy);
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
