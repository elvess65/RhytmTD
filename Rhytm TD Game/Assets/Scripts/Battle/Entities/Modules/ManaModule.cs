using System;
using UnityEngine;

namespace RhytmTD.Battle.Entities
{
    public class ManaModule : IBattleModule
    {
        public int CurrentMana { get; private set; }
        public int TotalMana { get; private set; }

        public Action<int> OnManaAdded;
        public Action<int> OnManaRemoved;
        

        public ManaModule(int totalAmount)
        {
            CurrentMana = TotalMana = totalAmount;
        }

        public void AddMana(int amount)
        {
            CurrentMana = Mathf.Min(CurrentMana + amount, TotalMana);
            OnManaAdded.Invoke(amount);
        }

        public void RemoveMana(int amount)
        {
            CurrentMana = Mathf.Max(CurrentMana - amount, 0);
            OnManaRemoved.Invoke(amount);
        }
    }
}
