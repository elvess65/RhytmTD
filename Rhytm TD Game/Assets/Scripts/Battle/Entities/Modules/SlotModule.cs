using RhytmTD.Battle.Entities.Views;
using UnityEngine;

namespace RhytmTD.Battle.Entities
{
    /// <summary>
    /// Contains spawn slots and is linked with view that holds references to slots in worlds
    /// Transmits slot's data from world to entity
    /// </summary>
    public class SlotModule : IBattleModule
    {
        public Transform ProjectileSlot { get; private set; }
        public Transform HitSlot { get; private set; }

        /// <summary>
        /// Contains spawn slots and is linked with view that holds references to slots in worlds
        /// Transmits slot's data from world to entity
        /// </summary>
        public SlotModule()
        {
        }

        public void InitializeModule(BattleEntityView entityView)
        {
            BattleEntitySlotView slotView = entityView.transform.GetComponent<BattleEntitySlotView>();

            ProjectileSlot = slotView.ProjectileSlot;
            HitSlot = slotView.HitSlot;
        }
    }
}
