using UnityEngine;

namespace RhytmTD.Battle.Entities
{
    /// <summary>
    /// Contains spawn slots and is linked with view its init
    /// </summary>
    public class SlotModule : IBattleModule
    {
        public Transform ProjectileSlot { get; private set; }
        public Transform HitSlot { get; private set; }

        public void Initialize(Transform projectileSlot, Transform hitSlot)
        {
            ProjectileSlot = projectileSlot;
            HitSlot = hitSlot;
        }
    }
}
