using System.Collections.Generic;
using UnityEngine;

public class PlacementData
{
    public int buildingDataID;
    public int guid;
    public List<Vector2Int> occupiedTiles = new();
    public GameObject prefab;
    public bool isFlip;
}