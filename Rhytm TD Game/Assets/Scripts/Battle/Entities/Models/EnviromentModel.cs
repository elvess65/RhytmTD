using System.Collections.Generic;
using CoreFramework;
using RhytmTD.Battle.Entities.Views;
using UnityEngine;

namespace RhytmTD.Battle.Entities.Models
{
    public class EnviromentModel : BaseModel
    {
        public System.Action OnInitializeEnviroment;
        public System.Action OnCellShouldBeAdded;
        public System.Action<EnviromentCellView> OnCellRemoved;

        public float SqrDistanceToSpawn = 0;
        public float SqrDistanceToRemove = 0;

        private List<EnviromentCellView> m_CellViews = new List<EnviromentCellView>();

        public Vector3 LastCellPos => m_LastCell != null ? m_LastCell.transform.position : Vector3.zero;
        public Vector3 LastCellFarEdgePos => m_LastCell != null ? m_LastCell.FarEdge.position : Vector3.zero;
        public Vector3 LastCellNearEdgePos => m_LastCell != null ? m_LastCell.NearEdge.position : Vector3.zero;
        public float LastCellLength => m_LastCell != null ? m_LastCell.Length : 0;
        public float LastCellSQRLength => m_LastCell != null ? m_LastCell.SQRLength : 0;

        public Vector3 FirstCellPos => m_FirstCell != null ? m_FirstCell.transform.position : Vector3.zero;
        public Vector3 FirstCellFarEdgePos => m_FirstCell != null ? m_FirstCell.FarEdge.position : Vector3.zero;
        public Vector3 FirstCellNearEdgePos => m_FirstCell != null ? m_FirstCell.NearEdge.position : Vector3.zero;
        public float FirstCellLength => m_FirstCell != null ? m_FirstCell.Length : 0;
        public float FirstCellSQRLength => m_FirstCell != null ? m_FirstCell.SQRLength : 0;

        private EnviromentCellView m_LastCell => m_CellViews.Count > 0 ? m_CellViews[m_CellViews.Count - 1] : null;
        private EnviromentCellView m_FirstCell => m_CellViews.Count > 0 ? m_CellViews[0] : null;

        private const float m_SQR_DIST_TO_SPAWN_MLTP = 2;
        private const float m_SQR_DIST_TO_REMOVE_MLTP = 2f;


        public void AddCell(EnviromentCellView cell)
        {
            m_CellViews.Add(cell);
        }

        public void RemoveCell()
        {
            EnviromentCellView cell = m_CellViews[0];
            m_CellViews.RemoveAt(0);

            OnCellRemoved?.Invoke(cell);
        }

        public void RecalculateActionDistances()
        {
            SqrDistanceToSpawn = LastCellSQRLength * m_SQR_DIST_TO_SPAWN_MLTP;
            SqrDistanceToRemove = LastCellSQRLength * m_SQR_DIST_TO_REMOVE_MLTP;
        }
    }
}
