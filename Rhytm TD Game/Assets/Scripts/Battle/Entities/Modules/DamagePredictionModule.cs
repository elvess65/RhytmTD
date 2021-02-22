
using System;

namespace RhytmTD.Battle.Entities
{
    /// <summary>
    /// Holds pottential damage that entity can deal to its target
    /// Is part of "Enemy destroyed prediction system"
    /// </summary>
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

        /// <summary>
        /// Holds pottential damage that entity can deal to its target
        /// Is part of "Enemy destroyed prediction system"
        /// </summary>
        public DamagePredictionModule()
        {

        }
    }
}
