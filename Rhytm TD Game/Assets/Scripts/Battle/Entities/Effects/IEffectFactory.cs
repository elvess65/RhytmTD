
using UnityEngine;

namespace RhytmTD.Battle.Entities.Effects
{
    public interface IEffectFactory
    {
        BattleEntity CreateMeteoriteEffect(Vector3 position, Quaternion rotation, float moveSpeed);
        BattleEntity CreateFireballEffect(Vector3 position, Quaternion rotation, float moveSpeed);
    }
}
