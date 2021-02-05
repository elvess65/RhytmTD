using CoreFramework;
using CoreFramework.Abstract;
using RhytmTD.Battle.Entities.Models;
using UnityEngine;

namespace RhytmTD.Battle.Entities.Controllers
{
    /// <summary>
    /// Фокусировка на цели
    /// </summary>
    public class FocusController : BaseController, iUpdatable
    {
        private BattleModel m_BattleModel;

        public FocusController(Dispatcher dispatcher) : base(dispatcher)
        {
        }

        public override void InitializeComplete()
        {
            m_BattleModel = Dispatcher.GetModel<BattleModel>();
        }

        public void PerformUpdate(float deltaTime)
        {
            FocusAll(deltaTime);
        }


        private void FocusAll(float deltaTime)
        {
            foreach (BattleEntity entity in m_BattleModel.BattleEntities)
            {
                if (entity.HasModule<TransformModule>())
                {
                    TransformModule transformModule = entity.GetModule<TransformModule>();
                    
                    if (transformModule.IsFocusing)
                    {
                        Quaternion targetRotation = Quaternion.LookRotation(transformModule.FocusTarget.position - transformModule.Transform.position).normalized;
                        transformModule.Transform.rotation = Quaternion.Slerp(transformModule.Transform.rotation, targetRotation, deltaTime * transformModule.Speed);;
                    }
                }
            }
        }
    }
}
