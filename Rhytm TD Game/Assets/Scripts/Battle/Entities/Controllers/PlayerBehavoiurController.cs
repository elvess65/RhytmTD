using CoreFramework;
using RhytmTD.Battle.Entities.Models;
using UnityEngine;

namespace RhytmTD.Battle.Entities.Controllers
{
    public class PlayerBehavoiurController : BaseController
    {
        private BattleModel m_BattleModel;
        private DamagePredictionModule m_DamagePredictionModule;
        private TargetModule m_TargetModule;

        private TransformModule m_TargetTransformModule;
        private HealthModule m_TargetHealthModule;

        public PlayerBehavoiurController(Dispatcher dispatcher) : base(dispatcher)
        {
        }

        public override void InitializeComplete()
        {
            m_BattleModel = Dispatcher.GetModel<BattleModel>();

            m_BattleModel.OnPlayerEntityInitialized += PlayerInitializedHanlder;
        }

        private void PlayerInitializedHanlder(BattleEntity battleEntity)
        {
            m_DamagePredictionModule = battleEntity.GetModule<DamagePredictionModule>();
            m_DamagePredictionModule.OnPotentialDamageChanged += PotentialDamageChanged;

            m_TargetModule = battleEntity.GetModule<TargetModule>();
            m_TargetModule.OnTargetChanged += TargetChanged;

            TransformModule transformModlue = battleEntity.GetModule<TransformModule>();
            transformModlue.OnPositionChanged += PlayerPositionChanged;
        }

        private void PotentialDamageChanged(int potentialDamage)
        {
            if (!m_TargetModule.HasTarget)
                return;

            if (potentialDamage >= m_TargetHealthModule.Health)
            {
                m_TargetModule.Target.AddModule(new PredictedDestroyedTag());
                m_TargetModule.ClearTarget();
            }
        }

        private void TargetChanged(BattleEntity target)
        {
            if (target != null)
            {
                DestroyModule destroyModule = target.GetModule<DestroyModule>();
                destroyModule.OnDestroyed += TargetDestroyed;

                m_TargetTransformModule = target.GetModule<TransformModule>();
                m_TargetHealthModule = target.GetModule<HealthModule>();
            }

            m_DamagePredictionModule.PotentialDamage = 0;
        }

        private void TargetDestroyed(BattleEntity battleEntity)
        {
            m_TargetModule.ClearTarget();
        }

        private void PlayerPositionChanged(Vector3 position)
        {
            if (m_TargetModule.HasTarget)
            {
                if (position.z >= m_TargetTransformModule.Position.z)
                {
                    m_TargetModule.ClearTarget();
                }
            }
        }
    }
}
