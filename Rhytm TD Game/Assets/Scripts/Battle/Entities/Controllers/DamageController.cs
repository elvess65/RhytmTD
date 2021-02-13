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

        public void DealDamage(int senderID, int receiverID, int damage)
        {
            BattleEntity receiver = m_BattleModel.GetEntity(receiverID);
            HealthModule receiverHealthModule = receiver.GetModule<HealthModule>();

            receiverHealthModule.RemoveHealth(damage, senderID);
        }

        public void DealDamage(int senderID, int receiverID)
        {
            BattleEntity sender = m_BattleModel.GetEntity(senderID);
            DamageModule senderDamageModule = sender.GetModule<DamageModule>();

            int damage = Random.Range(senderDamageModule.MinDamage, senderDamageModule.MaxDamage);

            DealDamage(senderID, receiverID, damage);
        }
    }
}
