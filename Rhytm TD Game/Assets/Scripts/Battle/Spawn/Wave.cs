using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave 
{
    public int m_WaveID { get; }         //ID волны
    public int m_EnemiesAmount { get; }    //Количество врагов в волне
    public int m_AttackTicks { get; }     //Количество тиков за которое враги должны быть созданы
    public int m_RestTicks { get; }       //Количество тиков после создания всех врагов до начала следующей волны

    public Wave(int id, int enemiesAmount, int attackTicks, int restTicks)
    {
        m_WaveID = id;
        m_EnemiesAmount = enemiesAmount;
        m_AttackTicks = attackTicks;
        m_RestTicks = restTicks;
    }

    public override string ToString()
    {
        return $"Wave {m_WaveID}. Enemies: {m_EnemiesAmount}. Attack Ticks: {m_AttackTicks}. Rest Ticks: {m_RestTicks}";
    }
}
