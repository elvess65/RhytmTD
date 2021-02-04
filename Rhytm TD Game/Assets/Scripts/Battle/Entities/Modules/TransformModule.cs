using UnityEngine;

namespace RhytmTD.Battle.Entities
{
    public class TransformModule : IBattleModule
    {
        public Transform Transform { get; private set; }

        public TransformModule(Transform transform)
        {
            Transform = transform;
        }
    }
}
