using CoreFramework;
using RhytmTD.Battle.Entities.Models;
using System.Collections.Generic;
using UnityEngine;

namespace RhytmTD.Battle.Entities.Controllers
{
    /// <summary>
    /// Controls selecting directions and areas for attacks as well as detecting targets
    /// </summary>
    public class TargetingController : BaseController
    {
        private BattleModel m_BattleModel;
        private CameraModel m_CameraModel;

        private List<BattleEntity> m_PotentialTargets;


        public TargetingController(Dispatcher dispatcher) : base(dispatcher)
        {
            m_PotentialTargets = new List<BattleEntity>();
        }

        public override void InitializeComplete()
        {
            base.InitializeComplete();

            m_BattleModel = Dispatcher.GetModel<BattleModel>();
            m_CameraModel = Dispatcher.GetModel<CameraModel>();
        }


        public Vector3 GetDirection(Vector3 mouseScreenPos, Vector3 anchorWorldPos, out Vector3 hitPos)
        {
            anchorWorldPos.y = 0;

            if (RaycastGround(mouseScreenPos, out hitPos))
            {
                return (hitPos - anchorWorldPos).normalized;
            }

            return Vector3.forward;
        }

        public BattleEntity GetTargetForDirectionBaseAttack(Vector3 fromWorldPos, Vector3 playerPos, Vector3 attackDir)
        {
            BattleEntity target = null;
            fromWorldPos.y = 0;

            //Dispose container
            if (m_PotentialTargets.Count > 0)
                m_PotentialTargets.Clear();

            //Get potential targets
            foreach (BattleEntity entity in m_BattleModel.BattleEntities)
            {
                if (!entity.HasModule<EnemyBehaviourTag>())
                    continue;

                //Get from module
                float enemyColliderSize = 1.5f;
                float distance2ShootLine = GetDistanceToLine(attackDir, fromWorldPos, entity.GetModule<TransformModule>().Position);

                if (distance2ShootLine <= enemyColliderSize)
                    m_PotentialTargets.Add(entity);
            }

            if (m_PotentialTargets.Count > 0)
            {
                if (m_PotentialTargets.Count > 1)
                {
                    //Filter target by distance if mo than 1
                    float closestSqrDist2Enemy = float.MaxValue;
                    foreach (BattleEntity potentialTarget in m_PotentialTargets)
                    {
                        TransformModule potentialTargetTransformModule = potentialTarget.GetModule<TransformModule>();

                        float sqrDist = (potentialTargetTransformModule.Position - playerPos).sqrMagnitude;
                        if (sqrDist < closestSqrDist2Enemy)
                        {
                            closestSqrDist2Enemy = sqrDist;
                            target = potentialTarget;
                        }
                    }
                }
                else
                    target = m_PotentialTargets[0];
            }

            return target;
        }

        public BattleEntity GetTargetForTargetBaseAttack()
        {
            TargetModule targetModule = m_BattleModel.PlayerEntity.GetModule<TargetModule>();
            BattleEntity targetEntity;

            if (!targetModule.HasTarget)
            {
                targetEntity = GetNearestEnemy(m_BattleModel.PlayerEntity);

                if (targetEntity != null)
                {
                    targetModule.SetTarget(targetEntity);
                }
            }
            else
            {
                targetEntity = targetModule.Target;
            }

            return targetEntity;
        }

        public BattleEntity GetNearestEnemy(BattleEntity sender)
        {
            TransformModule senderTransform = sender.GetModule<TransformModule>();
            float nearestDistance = float.MaxValue;
            BattleEntity retValue = null;

            foreach (BattleEntity entity in m_BattleModel.BattleEntities)
            {
                if (entity.ID == sender.ID || !entity.HasModule<EnemyBehaviourTag>() || entity.HasModule<PredictedDestroyedTag>())
                    continue;

                DestroyModule destroyModule = entity.GetModule<DestroyModule>();

                if (destroyModule.IsDestroyed)
                    continue;

                TransformModule transformModule = entity.GetModule<TransformModule>();

                if (senderTransform.Position.z >= transformModule.Position.z)
                    continue;

                float distanceSqr = (senderTransform.Position - transformModule.Position).sqrMagnitude;
                if (distanceSqr <= nearestDistance)
                {
                    nearestDistance = distanceSqr;
                    retValue = entity;
                }
            }

            return retValue;
        }


        private float GetDistanceToLine(Vector3 dir, Vector3 origin, Vector3 point)
        {
            return Vector3.Cross(dir, point - origin).magnitude;
        }

        private bool RaycastGround(Vector3 mouseScreenPos, out Vector3 hitPos)
        {
            hitPos = Vector3.zero;
            Ray ray = m_CameraModel.MainCamera.ScreenPointToRay(mouseScreenPos);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                hitPos = hit.point;
                hitPos.y = 0;

                return true;
            }

            return false;
        }
    }
}
