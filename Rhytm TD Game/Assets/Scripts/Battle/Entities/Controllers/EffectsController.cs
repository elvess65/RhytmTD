using CoreFramework;
using RhytmTD.Battle.Entities.Effects;
using RhytmTD.Battle.Entities.Models;
using UnityEngine;

namespace RhytmTD.Battle.Entities.Controllers
{
    public class EffectsController : BaseController
    {
        private EffectsModel m_EffectsModel;
        
        private IEffectEntityFactory m_EffectFactory;

        public EffectsController(Dispatcher dispatcher) : base(dispatcher)
        {
            m_EffectFactory = new DefaultEffectEntityFactory();
        }

        public override void InitializeComplete()
        {
            base.InitializeComplete();

            m_EffectsModel = Dispatcher.GetModel<EffectsModel>();
        }


        public BattleEntity CreateMeteoriteEffect(Vector3 position, Quaternion rotation, float moveSpeed)
        {
            BattleEntity battleEntity = m_EffectFactory.CreateMeteoriteEffectEntity(position, rotation, moveSpeed);
            m_EffectsModel.OnEffectEntityCreated?.Invoke(battleEntity);

            return battleEntity;
        }

        public BattleEntity CreateFireballEffect(Vector3 position, Quaternion rotation, float moveSpeed)
        {
            BattleEntity battleEntity = m_EffectFactory.CreateFireballEffectEntity(position, rotation, moveSpeed);
            m_EffectsModel.OnEffectEntityCreated?.Invoke(battleEntity);

            return battleEntity;
        }

        public BattleEntity CreateBulletEffect(EnumsCollection.BattlEffectID typeID, Vector3 position, Quaternion rotation, float speed, BattleEntity owner)
        {
            BattleEntity battleEntity = m_EffectFactory.CreateBulletEntity(typeID, position, rotation, speed, owner);
            m_EffectsModel.OnEffectEntityCreated?.Invoke(battleEntity);

            return battleEntity;
        }
    }
}
