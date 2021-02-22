
namespace RhytmTD.Battle.Entities
{
    /// <summary>
    /// Holds data about fireball skill
    /// </summary>
    public class FireballSkillModule : IBattleModule
    {
        public float MoveSpeed { get; }
        public int Damage { get; }

        /// <summary>
        /// Holds data about fireball skill
        /// </summary>
        public FireballSkillModule(float moveSpeed, int damage)
        {
            MoveSpeed = moveSpeed;
            Damage = damage;
        }
    }
}
