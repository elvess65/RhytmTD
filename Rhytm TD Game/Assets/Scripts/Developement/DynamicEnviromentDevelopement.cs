using RhytmTD.Battle.Entities.Views;
using UnityEngine;

public class DynamicEnviromentDevelopement : MonoBehaviour
{
    public Transform Target;
    public Transform Parent;  
    public EnviromentCellView CellPrefab;

    private EnviromentCellView m_LastCell;
    private float m_SqrDistanceToSpawn = 0;

    private const float m_SQR_DIST_TO_SPAWN_MULTIPLAYER = 1;

    void Start()
    {
        m_LastCell = FindObjectOfType<EnviromentCellView>();

        m_SqrDistanceToSpawn = (m_LastCell.FarEdge.position - m_LastCell.NearEdge.position).sqrMagnitude * m_SQR_DIST_TO_SPAWN_MULTIPLAYER;

        SpawnCell();
    }

    void Update()
    {
        float sqrDistToFarEdge = (m_LastCell.FarEdge.position - Target.position).sqrMagnitude;
        Debug.Log(sqrDistToFarEdge + " " + m_SqrDistanceToSpawn);

        if (sqrDistToFarEdge <= m_SqrDistanceToSpawn)
        {
            SpawnCell();
        }
    }

    void SpawnCell()
    {
        float spawnOffset = m_LastCell != null ? Vector3.Distance(m_LastCell.FarEdge.position, m_LastCell.NearEdge.position) : 0;
        Vector3 spawnPos = m_LastCell != null ? m_LastCell.transform.position : Vector3.zero;

        EnviromentCellView cell = Instantiate(CellPrefab);
        cell.transform.SetParent(Parent);
        cell.transform.localScale = Vector3.one;
        cell.transform.position = spawnPos - Vector3.forward * spawnOffset;

        m_LastCell = cell;
    }
}
