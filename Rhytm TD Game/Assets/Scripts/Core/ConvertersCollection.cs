using System;
using CoreFramework;

namespace RhytmTD.Core
{
    public static class ConvertersCollection
    { 
        public static DateTime ConvertFromUnixTimestamp(double timestamp)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            return origin.AddSeconds(timestamp);
        }

        public static double ConvertToUnixTimestamp(DateTime date)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            TimeSpan diff = date.ToUniversalTime() - origin;
            return Math.Floor(diff.TotalSeconds);
        }

        public static EnumsCollection.AnimationTypes ConvertSkillTypeID2AnimationType(int skillTypeID)
        {
            switch(skillTypeID)
            {
                case ConstsCollection.SkillConsts.FIREBALL_ID:
                    return EnumsCollection.AnimationTypes.UseWeaponSkill;
                case ConstsCollection.SkillConsts.METEORITE_ID:
                    return EnumsCollection.AnimationTypes.UseCastableSkill;
                case ConstsCollection.SkillConsts.HEALTH_ID:
                    return EnumsCollection.AnimationTypes.UseCastableSkill;
            }

            return EnumsCollection.AnimationTypes.Attack;
        }
    }
}
