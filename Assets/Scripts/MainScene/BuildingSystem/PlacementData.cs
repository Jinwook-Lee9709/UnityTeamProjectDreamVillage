using System.Collections.Generic;
using UnityEngine;

public class PlacementData
{
    public int buildingDataId;
    public int guid;
    public Vector3Int pivotPoint;
    public List<Vector2Int> occupiedTiles = new();
    public GameObject prefab;
    public bool isFlip;
}