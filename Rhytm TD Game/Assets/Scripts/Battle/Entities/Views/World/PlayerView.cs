using RhytmTD.Battle.Entities.Controllers;
using UnityEngine;
using static CoreFramework.EnumsCollection;

namespace RhytmTD.Battle.Entities.Views
{
    public class PlayerView : BattleEntityView
    {
        [SerializeField] private BattleEntityView[] ViewsToInit;

        private AnimationModule m_AnimationModule;
        private MarkerController m_MarkerController;
        private CameraController m_CameraController;

        private int m_TargetMarkerID;
        private bool m_MarkerShowed = false;

        public override void Initialize(BattleEntity entity)
        {
            base.Initialize(entity);

            m_MarkerController = Dispatcher.GetController<MarkerController>();
            m_CameraController = Dispatcher.GetController<CameraController>();
            m_CameraController.SetTarget(transform);

            m_AnimationModule = entity.GetModule<AnimationModule>();

            MoveModule moveModule = entity.GetModule<MoveModule>();
            moveModule.OnMoveStarted += OnMoveStarted;
            moveModule.OnMoveStopped += OnMoveStopped;

            TransformModule transformModule = entity.GetModule<TransformModule>();
            transformModule.OnPositionChanged += OnPositionChanged;

            HealthModule healthModule = entity.GetModule<HealthModule>();
            healthModule.OnHealthRemoved += OnHealthRemoved;

            DestroyModule destroyModule = entity.GetModule<DestroyModule>();
            destroyModule.OnDestroyed += OnDestroyed;

            TargetModule targetModule = entity.GetModule<TargetModule>();
            targetModule.OnTargetChanged += TargetChanged;

            foreach (BattleEntityView view in ViewsToInit)
            {
                view.Initialize(entity);
            }
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

        private void TargetChanged(BattleEntity target)
        {
            if (target != null)
            {
                m_TargetMarkerID = m_MarkerController.ShowTargetMarker(MarkerTypes.Target, target);
                m_MarkerShowed = true;
            }
            else if (m_MarkerShowed)
            {
                m_MarkerController.HideMarker(m_TargetMarkerID);
                m_MarkerShowed = false;
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position + new Vector3(0, 1.5f, 0), 2);
        }
    }
}
