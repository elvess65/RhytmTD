namespace RhytmTD
{
    public static class ConstsCollection
    {
        public const float SPELLBOOK_SPEED_MULTIPLAYER = 0.0f;
        public const float SPELLBOOK_AUDIO_TRANISTION_DURATION = 0.5f;
        public const int DDRP_INPUT_INCREASE = 1;
        public const int DDRP_INPUT_DECREASE = 2;

        public static class CharacterConsts
        {
            public const int MAGE_ID = 1000;
        }

        public static class EffectConsts
        {
            public const int ProjectileArrow = 500;
            public const int Meteorite = 501;
            public const int Fireball = 502;
            public const int Health = 503;
        }

        public static class SkillConsts
        {
            public const int METEORITE_ID = 100;
            public const int FIREBALL_ID = 101;
            public const int HEALTH_ID = 102;

            public const int PATTERN_1_ID = 1100;
            public const int PATTERN_2_ID = 1101;
            public const int PATTERN_3_ID = 1102;
        } 

        public static class DataConsts
        {
            public const string ACTION = "a";
            public const string EXPLOSION = "e";
            public const string MUZZLE = "m";
            public const string RADIUS = "r";
        }
    }
}
