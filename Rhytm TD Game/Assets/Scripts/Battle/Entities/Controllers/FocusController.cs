using CoreFramework;
using RhytmTD.Battle.Entities.Models;
using RhytmTD.Data.Models;
using UnityEngine;

namespace RhytmTD.Battle.Entities.Controllers
{
    /// <summary>
    /// Фокусировка на цели
    /// </summary>
    public class FocusController : BaseController
    {
        private BattleModel m_BattleModel;

        public FocusController(Dispatcher dispatcher) : base(dispatcher)
        {
        }

        public override void InitializeComplete()
        {
            m_BattleModel = Dispatcher.GetModel<BattleModel>();

            UpdateModel updateModel = Dispatcher.GetModel<UpdateModel>();
            updateModel.OnUpdate += Update;
        }

        public void Update(float deltaTime)
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
