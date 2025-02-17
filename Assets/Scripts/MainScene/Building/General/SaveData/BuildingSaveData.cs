using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BuildingSaveData
{
    public Vector3Int position;
    public int buildingId;
    public bool isFlip;
    public BuildingTaskData task;
}