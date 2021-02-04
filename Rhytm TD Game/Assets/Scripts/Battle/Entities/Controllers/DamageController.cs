using CoreFramework;
using RhytmTD.Battle.Entities.Models;
using UnityEngine;

namespace RhytmTD.Battle.Entities.Controllers
{
    /// <summary>
    /// Нанесение урона
    /// </summary>
    public class DamageController : BaseController
    {
        private BattleModel m_BattleModel;

        public DamageController(Dispatcher dispatcher) : base(dispatcher)
        {
        }

        public override void InitializeComplete()
        {
            m_BattleModel = Dispatcher.GetModel<BattleModel>();
        }

        public void DealDamage(int senderID, int receiverID)
        {
            BattleEntity sender = m_BattleModel.GetEntity(senderID);
            BattleEntity receiver = m_BattleModel.GetEntity(receiverID);

            DamageModule senderDamageModule = sender.GetModule<DamageModule>();
            HealthModule receiverHealthModule = receiver.GetModule<HealthModule>();

            int damage = Random.Range(senderDamageModule.MinDamage, senderDamageModule.MaxDamage);

            receiverHealthModule.RemoveHealth(damage, senderID);
        }
    }
}
