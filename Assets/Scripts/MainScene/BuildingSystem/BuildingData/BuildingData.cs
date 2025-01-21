using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BuildingData
{
    public string name = "Test";
    public int level = 0;
    public int exp = 0;
    public float productionTime = 0f;
    public int cost = 0;
    public Vector2 size = Vector2.zero;
    public GameObject prefab = null;
}
