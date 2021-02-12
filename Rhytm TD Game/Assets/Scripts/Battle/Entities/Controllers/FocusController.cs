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
                if (entity.HasModule<FocusModule>())
                {
                    FocusModule focusModule = entity.GetModule<FocusModule>();
                    
                    if (focusModule.IsFocusing)
                    {
                        TransformModule transformModule = entity.GetModule<TransformModule>();
                        RotateModule rotateModule = entity.GetModule<RotateModule>();

                        Quaternion targetRotation = Quaternion.LookRotation((focusModule.TargetTransform.Position - transformModule.Position).normalized);
                        rotateModule.StartRotate(targetRotation);
                    }
                }
            }
        }
    }
}
