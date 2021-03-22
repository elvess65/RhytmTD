using System;
using UnityEngine;

namespace RhytmTD.Battle.Entities
{
    public class ManaModule : IBattleModule
    {
        public int CurrentAmount { get; private set; }
        public int TotalAmount { get; private set; }

        public Action<int> OnManaAdded;
        public Action<int> OnManaRemoved;
        

        public ManaModule(int totalAmount)
        {
            CurrentAmount = TotalAmount = totalAmount;
        }

        public void AddMana(int amount)
        {
            CurrentAmount = Mathf.Min(CurrentAmount + amount, TotalAmount);
            OnManaAdded.Invoke(amount);
        }

        public void RemoveMana(int amount)
        {
            CurrentAmount = Mathf.Max(CurrentAmount - amount, 0);
            OnManaRemoved.Invoke(amount);
        }
    }
}
