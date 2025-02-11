using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BuildingData
{
    public BuildingTypes buildingType;
    public int level = 0;
    public int exp = 0;
    public float productionTime = 0f;
    public int cost = 0;
    public int population = 0;
    public Vector2Int size = Vector2Int.zero;
    public GameObject prefab = null;
}
