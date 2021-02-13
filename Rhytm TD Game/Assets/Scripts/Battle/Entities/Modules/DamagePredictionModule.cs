
using System;

namespace RhytmTD.Battle.Entities
{
    public class DamagePredictionModule : IBattleModule
    {
        private int m_PotentialDamage;

        public Action<int> OnPotentialDamageChanged;

        public int PotentialDamage
        {
            get { return m_PotentialDamage; }
            set
            {
                m_PotentialDamage = value;
                OnPotentialDamageChanged?.Invoke(value);
            }
        }
    }
}
