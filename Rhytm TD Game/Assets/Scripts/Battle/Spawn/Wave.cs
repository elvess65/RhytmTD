using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave 
{
    private int m_WaveID;           //ID волны
    private int m_EnemiesAmount;    //Количество врагов в волне
    private int m_AttackTicks;      //Количество тиков за которое враги должны быть созданы
    private int m_RestTicks;        //Количество тиков после создания всех врагов до начала следующей волны

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
