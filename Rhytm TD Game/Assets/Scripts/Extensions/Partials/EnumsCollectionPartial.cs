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
            ProjectileArrow,
            SkillMeterite,
            SkillFireball
        }

        public enum PlayerCharacterID
        {
            Mage
        }

    }
}