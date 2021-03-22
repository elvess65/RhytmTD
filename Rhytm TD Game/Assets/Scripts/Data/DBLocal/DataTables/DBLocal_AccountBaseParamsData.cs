using CoreFramework;
using UnityEngine;

namespace RhytmTD.Data.DataBaseLocal
{
    /// <summary>
    /// Базовые параметры аккаунта (без прокачки)
    /// </summary>
    [System.Serializable]
    [CreateAssetMenu(fileName = "New Local AccountBaseParamsData", menuName = "DBLocal/Account/AccountBaseParamsData", order = 101)]
    public class DBLocal_AccountBaseParamsData : ScriptableObject
    {
        public CharacterBaseData BaseCharacterData;
        public FireballSkillBaseData BaseFireballData;
        public MeteoriteSkillBaseData BaseMeteoriteData;
        public HealthSkillBaseData BaseHealthData;

        [System.Serializable]
        public class CharacterBaseData
        {
            public float MoveSpeedUnitsPerTick = 2;
            public float FocusSpeed = 3;
            public int MinDamage = 12;
            public int MaxDamage = 17;
            public int Health = 50;
            public int Mana = 10;
        }

        [System.Serializable]
        public abstract class SkillBaseData
        {
            public EnumsCollection.SkillTypeID TypeID;
            public EnumsCollection.SkillTargetingType TargetingType;
            public EnumsCollection.SkillSequencePatternID DefaultPatternID;
            public float ActivationTime = 1;
            public float UseTime = 1;
            public float FinishingTime = 1;
            public int CooldownTicks = 10;
            public int ManaCost = 2;
        }

        [System.Serializable]
        public class FireballSkillBaseData : SkillBaseData
        {
            [Header("Fireball")]
            public float MoveSpeed = 4;
            public int Damage = 20;
        }

        [System.Serializable]
        public class MeteoriteSkillBaseData : SkillBaseData
        {
            [Header("Meteorite")]
            public float DamageRadius = 5;
            public int Damage = 10;
            public Vector2 EffectOffset = new Vector2(7.7f, 7.7f);
        }

        [System.Serializable]
        public class HealthSkillBaseData : SkillBaseData
        {
            [Header("Health")]
            public float HealthPercent;
        }
    }
}
