//#define LOG

using System.Collections.Generic;
using RhytmTD.Battle.Entities.Views;
using UnityEngine;

public class DynamicEnviromentDevelopement : MonoBehaviour
{
    public Transform Target;
    public Transform Parent;  
    public EnviromentCellView CellPrefab;
    public int DefaultAmountOfCells = 0;

    private List<EnviromentCellView> m_CellViews = new List<EnviromentCellView>();

    private EnviromentCellView m_LastCell => m_CellViews[m_CellViews.Count - 1];
    private EnviromentCellView m_FirstCell => m_CellViews[0];

    private float m_SqrDistanceToSpawn = 0;
    private float m_SqrDistanceToRemove = 0;

    private const float m_SQR_DIST_TO_SPAWN_MLTP = 2;
    private const float m_SQR_DIST_TO_REMOVE_MLTP = 2f;

    void Start()
    {
        Initialize();
    }

    void Initialize()
    {
        m_CellViews.Add(FindObjectOfType<EnviromentCellView>());

        RecalculateActionDistances();

        for (int i = 0; i < DefaultAmountOfCells; i++)
        {
            AddCell();
        }
    }

    
    void Update()
    {
        float sqrDistToLastEdge = (m_LastCell.FarEdge.position - Target.position).sqrMagnitude;
        float sqrDistToFirstEdge = (m_FirstCell.NearEdge.position - Target.position).sqrMagnitude;

#if UNITY_EDITOR
        Vector3 offset = Vector3.up * 2;
        Debug.DrawLine(Target.position + offset, m_LastCell.FarEdge.position + offset, Color.green);
        Debug.DrawLine(Target.position + offset, m_FirstCell.NearEdge.position + offset, Color.red);
#endif

#if LOG

        Debug.Log("Create: (" + sqrDistToLastEdge + " : " + m_SqrDistanceToSpawn +
                ") Remove: (" + (sqrDistToFirstEdge + " : " + m_SqrDistanceToRemove));
#endif

        if (sqrDistToLastEdge <= m_SqrDistanceToSpawn)
        {
            AddCell();
        }

        if (sqrDistToFirstEdge >= m_SqrDistanceToRemove)
        {
            RemoveCell();
        }
    }



    private void AddCell()
    {
        EnviromentCellView cell = Instantiate(CellPrefab);
        cell.transform.SetParent(Parent);
        cell.transform.localScale = Vector3.one;
        cell.transform.position = m_LastCell.transform.position + Vector3.forward * m_LastCell.Length;

        m_CellViews.Add(cell);

        RecalculateActionDistances();
    }

    private void RemoveCell()
    {
        EnviromentCellView cell = m_CellViews[0];
        Destroy(cell.gameObject);

        m_CellViews.RemoveAt(0);

        RecalculateActionDistances();
    }


    private void RecalculateActionDistances()
    {
        m_SqrDistanceToSpawn = m_LastCell.SQRLength * m_SQR_DIST_TO_SPAWN_MLTP;
        m_SqrDistanceToRemove = m_LastCell.SQRLength * m_SQR_DIST_TO_REMOVE_MLTP;
    }

}
