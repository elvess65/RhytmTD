
using UnityEngine;

namespace RhytmTD.Battle.Entities
{
    public class HealthModule : IBattleModule
    {
        public int Health { get; private set; }
        public int CurrentHealth { get; private set; }
        public bool IsAlive => CurrentHealth > 0;

        public HealthModule(int health)
        {
            Health = CurrentHealth = health;
        }

        public void AddHealth(int health)
        {
            CurrentHealth += health;
        }

        public void RemoveHealth(int health)
        {
            CurrentHealth = Mathf.Max(CurrentHealth - health, 0);
        }
    }
}
