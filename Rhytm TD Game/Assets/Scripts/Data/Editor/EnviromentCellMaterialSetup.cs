using System.Collections.Generic;
using UnityEngine;
using static CoreFramework.EnumsCollection;

namespace RhytmTD.Data.Editor
{
    [CreateAssetMenu(fileName = "New EnviromentCell Material Setup", menuName = "Development/EnviromentCellMaterialSetup", order = 101)]
    public class EnviromentCellMaterialSetup : ScriptableObject
    {
        public List<EnviromentCellMaterialSetupData> EnviromentCellMaterials;

        [System.Serializable]
        public class EnviromentCellMaterialSetupData
        {
            public EnviromentTypes Type;
            public Material Material;
        }
    }
}
