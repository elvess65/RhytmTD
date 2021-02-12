
namespace RhytmTD.Battle.Entities.EntitiesFactory.Setups
{
    public class PlayerFactorySetup : EntityFactorySetup
    {
        public float MoveSpeed { get; }
        public int Mana { get; }

        public PlayerFactorySetup(float focusSpeed, int minDamage, int maxDamage, int health, float moveSpeed, int mana) : base(focusSpeed, minDamage, maxDamage, health)
        {
            MoveSpeed = moveSpeed;
            Mana = mana;
        }
    }
}
