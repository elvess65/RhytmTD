﻿using System.Collections.Generic;
using UnityEngine;

public class EnviromentCell : MonoBehaviour
{
    public enum EnviromentTypes
    {
        All, Ground, Tiles, Rocks, Logs, Trees, Vegetation, Props, GroundProps, Max
    }

    [SerializeField]
    public Dictionary<EnviromentTypes, List<MeshRenderer>> Renderers = new Dictionary<EnviromentTypes, List<MeshRenderer>>();
}
