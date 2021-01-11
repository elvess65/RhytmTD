﻿using UnityEngine;

namespace RhytmTD.Data.DataBase.Simulation
{
    [CreateAssetMenu(fileName = "New Simulation AccountData", menuName = "DBSimulation/AccountData", order = 101)]
    public class DBSimulation_AccountData : ScriptableObject
    {
        [Header("Данные аккаунта")]

        [Tooltip("Опыт оружия")]
        public int WeaponExperiance;

        [Tooltip("Опыт ХР")]
        public int HPExperiance;

        [Tooltip("Опыт маны")]
        public int ManaExperiance;
    }
}
