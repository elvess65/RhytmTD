using RhytmTD;

namespace CoreFramework
{
    public static partial class EnumsCollection
    {
        public enum AnimationTypes
        {
            Attack,
            TakeDamage,
            Destroy,
            StartMove,
            StopMove,
            IncreaseHP,
            Show,
            Hide,
            Victory,
            MenuAction,
            IdleBattle,
            IdleNormal,
            UseWeaponSkill, 
            UseCastableSkill,
        }

        public enum BattlEffectID
        {
            ProjectileArrow = ConstsCollection.EffectConsts.ProjectileArrow,
            SkillMeterite = ConstsCollection.EffectConsts.Meteorite,
            SkillFireball = ConstsCollection.EffectConsts.Fireball,
            SkillHealth = ConstsCollection.EffectConsts.Health
        }

        public enum SkillID
        {
            Meteorite = ConstsCollection.SkillConsts.METEORITE_ID,
            Fireball = ConstsCollection.SkillConsts.FIREBALL_ID,
            Health = ConstsCollection.SkillConsts.HEALTH_ID
        }

        public enum PlayerCharacterID
        {
            Mage = ConstsCollection.CharacterConsts.MAGE_ID
        }

        public enum CameraTypes
        {
            Default,
            Main
        }

        public enum MarkerTypes
        {
            AttackRadius,
            Target,
            AllyTarget
        }
    }
}