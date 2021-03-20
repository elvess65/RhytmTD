using System.Collections.Generic;
using CoreFramework;
using UnityEngine;
using static CoreFramework.EnumsCollection;

namespace RhytmTD.Battle.Entities.Views
{
    public class EnviromentCellView : BaseView
    {
        public Transform NearEdge;
        public Transform FarEdge;

        public float SQRLength { get; private set; }
        public float Length { get; private set; }

        public Dictionary<EnviromentTypes, List<MeshRenderer>> Renderers = new Dictionary<EnviromentTypes, List<MeshRenderer>>();

        private void Awake()
        {
            Initialize();
        }

        private void Initialize()
        {
            Vector3 lengthVec = FarEdge.position - NearEdge.position;
            SQRLength = lengthVec.sqrMagnitude;
            Length = lengthVec.magnitude;
        }

        private void OnDrawGizmos()
        {
            Color prevColor = Gizmos.color;

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(NearEdge.position + Vector3.up * 2, 1);

            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(FarEdge.position + Vector3.up * 2, 1);

            Gizmos.color = prevColor;
        }
    }
}
