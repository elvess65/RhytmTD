
using UnityEngine;

namespace RhytmTD.Battle.Entities
{
    public class HealthModule : IBattleModule
    {
        public int Health { get; private set; }
        public int CurrentHealth { get; private set; }
        public bool IsAlive => CurrentHealth > 0;

        public delegate void HealthRemovedEventHanlder(int amount, int senderID);
        public event HealthRemovedEventHanlder OnHealthRemoved;

        public HealthModule(int health)
        {
            Health = CurrentHealth = health;
        }

        public void AddHealth(int health)
        {
            CurrentHealth += health;
        }

        public void RemoveHealth(int health, int senderID)
        {
            CurrentHealth = Mathf.Max(CurrentHealth - health, 0);

            OnHealthRemoved?.Invoke(health, senderID);
        }
    }
}
