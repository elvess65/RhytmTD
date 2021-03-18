using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New EnviromentCell Setup", menuName = "Development/EnviromentCellSetup", order = 101)]
public class EnviromentCellEditorMaterialSetup : ScriptableObject
{
    //public EnviromentCellSetupData[] 

    [System.Serializable]     
    public class EnviromentTypeSetupData
    {
        public EnviromentCell.EnviromentTypes Type;
        public Material Material;
    }
}
