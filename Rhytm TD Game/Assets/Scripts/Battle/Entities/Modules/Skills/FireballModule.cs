
namespace RhytmTD.Battle.Entities
{
    public class FireballModule : IBattleModule
    {
        public float MoveSpeed { get; }
        public int Damage { get; }

        public FireballModule(float moveSpeed, int damage)
        {
            MoveSpeed = moveSpeed;
            Damage = damage;
        }
    }
}
