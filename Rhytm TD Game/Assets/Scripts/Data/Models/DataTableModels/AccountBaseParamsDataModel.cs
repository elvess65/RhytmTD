using CoreFramework;
using UnityEngine;

namespace RhytmTD.Data.Models.DataTableModels
{
    /// <summary>
    /// Базовые параметры аккаунта (без прокачки)
    /// </summary>
    public class AccountBaseParamsDataModel : BaseModel
    {
        public CharacterBaseData BaseCharacterData;
        public FireballSkillBaseData BaseFireballData;
        public MeteoriteSkillBaseData BaseMeteoriteData;

        [System.Serializable]
        public class CharacterBaseData
        {
            public float MoveSpeedUnitsPerTick;
            public float FocusSpeed;
            public int MinDamage;
            public int MaxDamage;
            public int Health;
            public int Mana;
        }

        [System.Serializable]
        public abstract class SkillBaseData
        {
            public EnumsCollection.SkillID TypeID;
            public float ActivationTime;
            public float UseTime;
            public float FinishingTime;
            public float CooldownTime;
        }

        [System.Serializable]
        public class FireballSkillBaseData : SkillBaseData
        {
            public float MoveSpeed;
            public int Damage;
        }

        [System.Serializable]
        public class MeteoriteSkillBaseData : SkillBaseData
        {
            public float FlyTime;
            public float DamageRadius;
            public int Damage;
            public Vector2 EffectOffset;
        }
    }
}
