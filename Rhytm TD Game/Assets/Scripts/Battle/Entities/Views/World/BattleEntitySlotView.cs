using UnityEngine;

namespace RhytmTD.Battle.Entities.Views
{
    /// <summary>
    /// View for reference slots
    /// </summary>
    public class BattleEntitySlotView : BattleEntityView
    {
        public Transform ProjectileSlot;
        public Transform HitSlot;

        public override void Initialize(BattleEntity entity)
        {
            base.Initialize(entity);

            SlotModule slotModule = entity.GetModule<SlotModule>();
            slotModule.SetTransforms(ProjectileSlot, HitSlot);
        }
    }
}
