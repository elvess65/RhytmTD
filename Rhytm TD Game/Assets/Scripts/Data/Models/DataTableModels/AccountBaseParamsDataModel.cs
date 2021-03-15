using CoreFramework;
using System.Collections.Generic;
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
        public HealthSkillBaseData BaseHealthData;

        private Dictionary<EnumsCollection.SkillTypeID, SkillBaseData> m_SkillBaseData;

        public SkillBaseData GetSkillBaseDataByID(int typeID)
        {
            EnumsCollection.SkillTypeID skillTypeID = (EnumsCollection.SkillTypeID)typeID;
            return m_SkillBaseData[skillTypeID];
        }

        public override  void Initialize()
        {
            base.Initialize();

            m_SkillBaseData = new Dictionary<EnumsCollection.SkillTypeID, SkillBaseData>();
            m_SkillBaseData.Add(BaseFireballData.TypeID, BaseFireballData);
            m_SkillBaseData.Add(BaseMeteoriteData.TypeID, BaseMeteoriteData);
            m_SkillBaseData.Add(BaseHealthData.TypeID, BaseHealthData);
        }

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
            public EnumsCollection.SkillTypeID TypeID;
            public EnumsCollection.SkillTargetingType TargetingType;
            public EnumsCollection.SkillSequencePatternID DefaultPatternID;
            public float ActivationTime;
            public float UseTime;
            public float FinishingTime;
            public int CooldownTicks;
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
            public float DamageRadius;
            public int Damage;
            public Vector2 EffectOffset;
        }

        [System.Serializable]
        public class HealthSkillBaseData : SkillBaseData
        {
            public float HealthPercent;
        }
    }
}
