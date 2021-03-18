using System.Collections.Generic;
using UnityEngine;
using static CoreFramework.EnumsCollection;

namespace RhytmTD.Battle.Entities.Views
{
    public class EnviromentCellView : MonoBehaviour
    {
        public Transform NearEdge;
        public Transform FarEdge;

        public Dictionary<EnviromentTypes, List<MeshRenderer>> Renderers = new Dictionary<EnviromentTypes, List<MeshRenderer>>();
    }
}
