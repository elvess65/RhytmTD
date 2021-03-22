using CoreFramework;
using RhytmTD.Battle.Entities.Models;
using RhytmTD.Data.Models;
using UnityEngine;

namespace RhytmTD.Battle.Entities.Controllers
{
    public class RotateController : BaseController
    {
        private BattleModel m_BattleModel;
        private SpellBookModel m_SpellBookModel;

        public RotateController(Dispatcher dispatcher) : base(dispatcher)
        {
        }

        public override void InitializeComplete()
        {
            m_BattleModel = Dispatcher.GetModel<BattleModel>();
            m_SpellBookModel = Dispatcher.GetModel<SpellBookModel>();

            UpdateModel updateModel = Dispatcher.GetModel<UpdateModel>();
            updateModel.OnUpdate += Update;
        }

        public void Update(float deltaTime)
        {
            RotateAll(deltaTime);
        }

        private void RotateAll(float deltaTime)
        {
            foreach (BattleEntity entity in m_BattleModel.BattleEntities)
            {
                if (entity.HasModule<RotateModule>())
                {
                    RotateModule rotateModule = entity.GetModule<RotateModule>();
                    
                    if (rotateModule.IsRotating)
                    {
                        TransformModule transformModule = entity.GetModule<TransformModule>();
                        transformModule.Rotation = Quaternion.Slerp(transformModule.Rotation, rotateModule.Destination, 
                                                                    deltaTime * rotateModule.CurrentSpeed * m_SpellBookModel.SpeedMultiplayer);
                    }
                }
            }
        }
    }
}
