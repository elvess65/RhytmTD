﻿using UnityEngine;

namespace RhytmTD.Data.DataBase.Simulation
{
    /// <summary>
    /// Local data base 
    /// </summary>
    [System.Serializable]
    public class DBSimulation : MonoBehaviour
    {
        public DBSimulation_AccountData AccountData;
        public DBSimulation_AccountLevelingData AccountLevelingData;
        public DBSimulation_EnvironmentData EnvironmentData;
    }
}