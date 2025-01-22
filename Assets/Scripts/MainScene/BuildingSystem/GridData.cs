using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class GridData
{
    Dictionary<Vector3Int,PlacementData> placedObects = new Dictionary<Vector3Int,PlacementData>();

    public void AddObject(int guid, Vector3Int position, BuildingData buildingData, bool isFlip )
    {
        PlacementData data = new();
        data.id = guid;
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
        var objects = placedObects.Where(x => x.Value.id == guid);
        foreach (var obj in objects)
        {
            placedObects.Remove(obj.Key);
        }
    }

    public int GetGuid(Vector3Int position)
    {
        placedObects.TryGetValue(position, out var data);
        if (data == null)
        {
            return -1;
        }
        return data.id;
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
