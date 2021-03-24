using RhytmTD.Battle.Entities.Controllers;
using RhytmTD.Battle.Entities.Models;
using UnityEngine;
using static CoreFramework.EnumsCollection;

namespace RhytmTD.Battle.Entities.Views
{
    public class PlayerView : BattleEntityView
    {
        [SerializeField] private BattleEntityView[] ViewsToInit = null;

        private MarkerController m_MarkerController;
        private CameraController m_CameraController;

        private AnimationModule m_AnimationModule;
        private MoveModule m_MoveModule;
        private TransformModule m_TransformModule;
        private HealthModule m_HealthModule;
        private DestroyModule m_DestroyModule;
        private TargetModule m_TargetModule;

        private int m_TargetMarkerID;
        private bool m_MarkerShowed = false;

        public override void Initialize(BattleEntity entity)
        {
            base.Initialize(entity);

            m_MarkerController = Dispatcher.GetController<MarkerController>();
            m_CameraController = Dispatcher.GetController<CameraController>();
            m_CameraController.SetTarget(transform);

            StartBattleSequenceModel startBattleSequenceModel = Dispatcher.GetModel<StartBattleSequenceModel>();
            startBattleSequenceModel.PlayerViewTransform = transform;

            m_AnimationModule = entity.GetModule<AnimationModule>();

            m_MoveModule = entity.GetModule<MoveModule>();
            m_MoveModule.OnMoveStarted += OnMoveStarted;
            m_MoveModule.OnMoveStopped += OnMoveStopped;

            m_TransformModule = entity.GetModule<TransformModule>();
            m_TransformModule.OnPositionChanged += OnPositionChanged;

            m_HealthModule = entity.GetModule<HealthModule>();
            m_HealthModule.OnHealthRemoved += OnHealthRemoved;

            m_DestroyModule = entity.GetModule<DestroyModule>();
            m_DestroyModule.OnDestroyed += OnDestroyed;

            m_TargetModule = entity.GetModule<TargetModule>();
            m_TargetModule.OnTargetChanged += TargetChanged;

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
                TransformModule transformModule = target.GetModule<TransformModule>();

                m_TargetMarkerID = m_MarkerController.ShowMarkerAtPosition(MarkerTypes.Target, transformModule.Position);
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

        private void OnDestroy()
        {
            m_MoveModule.OnMoveStarted -= OnMoveStarted;
            m_MoveModule.OnMoveStopped -= OnMoveStopped;

            m_TransformModule.OnPositionChanged -= OnPositionChanged;

            m_HealthModule.OnHealthRemoved -= OnHealthRemoved;

            m_DestroyModule.OnDestroyed -= OnDestroyed;

            m_TargetModule.OnTargetChanged -= TargetChanged;
        }
    }
}
