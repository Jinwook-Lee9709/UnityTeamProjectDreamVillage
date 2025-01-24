using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GridData
{
    Dictionary<Vector3Int,PlacementData> placedObects = new Dictionary<Vector3Int,PlacementData>();
    
    public void AddObject(int buildingDataId, int guid, Vector3Int position, BuildingData buildingData, bool isFlip )
    {
        PlacementData data = new();
        data.buildingDataID = buildingDataId;
        data.guid = guid;
        Vector2 size = isFlip ? new Vector2(buildingData.size.y, buildingData.size.x) : new Vector2(buildingData.size.x, buildingData.size.y);
        for (int i = 0; i < buildingData.size.x; i++)
        {
            for (int j = 0; j < buildingData.size.y; j++)
            {
                data.occupiedTiles.Add(new Vector2Int(position.x + i, position.z + j));
            }
        }
        data.prefab = buildingData.prefab;
        data.isFlip = isFlip;
        foreach (var pos in data.occupiedTiles)
        {
            placedObects.Add(new Vector3Int(pos.x,position.y,pos.y), data);
        }

    }
    public void RemoveObject(int guid)
    {
        var keysToRemove = placedObects
            .Where(x => x.Value.guid == guid)
            .Select(x => x.Key)
            .ToList();
        
        foreach (var key in keysToRemove)
        {
            placedObects.Remove(key);
        }
    }
    public int GetGuid(Vector3Int position)
    {
        placedObects.TryGetValue(position, out var data);
        if (data == null)
        {
            return -1;
        }
        return data.guid;
    }
    
    public int GetBuildingDataId(int guid)
    {
        return placedObects.Where(x => x.Value.guid == guid).First().Value.buildingDataID;
    }

    public bool GetIsFliped(int guid)
    {
        return placedObects.Where(x => x.Value.guid == guid).First().Value.isFlip;
    }
    
    public bool IsValid(Vector3Int position, Vector2Int size)
    {
        for (int i = 0; i < size.x; i++)
        {
            for (int j = 0; j < size.y; j++)
            {
                if (placedObects.ContainsKey(position + new Vector3Int(i, 0, j)))
                    return false;
            }
        }
        return true;
    }
}
