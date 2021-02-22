using CoreFramework;
using System;

namespace RhytmTD.Battle.Entities.Models
{
    public class EffectsModel : BaseModel
    {
        public Action<BattleEntity> OnEffectEntityCreated;
    }
}
