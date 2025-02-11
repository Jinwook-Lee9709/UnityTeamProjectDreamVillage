using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering.UI;

public class GridData
{
    BuildingDatabaseSO buildingDatabase;
    Dictionary<Vector3Int, PlacementData> placedObects = new ();

    private GameObject constructionPrefab;

    public GridData()
    {
        string soPath = String.Format(PathFormat.soPath, SoIds.BuildingDatabase.ToString());
        buildingDatabase = Resources.Load<BuildingDatabaseSO>(soPath);
    }

    public void AddObject(int guid, int buildingDataId, Vector3Int position, BuildingData buildingData, bool isFlip, bool isFirst = true)
    {
        PlacementData data = new();
        data.guid = guid;
        data.pivotPoint = position;
        Vector2 size = isFlip
            ? new Vector2(buildingData.size.y, buildingData.size.x)
            : new Vector2(buildingData.size.x, buildingData.size.y);
        for (int i = 0; i < size.x; i++)
        {
            for (int j = 0; j < size.y; j++)
            {
                data.occupiedTiles.Add(new Vector2Int(position.x + i, position.z + j));
            }
        }

        if (buildingData.productionTime != 0 && isFirst)
        {
            int constructionBuildingId = Variables.constructionBuildingId + (buildingData.size.x - 1) * 2 + (buildingData.size.y - 1);
            data.buildingDataId = constructionBuildingId;
            data.prefab = buildingDatabase.Get(constructionBuildingId).prefab;
        }
        else
        {
            data.buildingDataId = buildingDataId;
            data.prefab = buildingData.prefab;
        }
        
        data.isFlip = isFlip;
        foreach (var pos in data.occupiedTiles)
        {
            placedObects.Add(new Vector3Int(pos.x, position.y, pos.y), data);
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

    public void ChangeObject(int guid, int buildingDataId)
    {
        var query = placedObects
            .Where(x => x.Value.guid == guid);

        foreach (var data in query)
        {
            data.Value.buildingDataId = buildingDataId;
            data.Value.prefab = buildingDatabase.Get(buildingDataId).prefab;
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
        return placedObects.Where(x => x.Value.guid == guid).First().Value.buildingDataId;
    }

    public bool GetIsFliped(int guid)
    {
        return placedObects.Where(x => x.Value.guid == guid).First().Value.isFlip;
    }

    public PlacementData GetPlacementData(int guid)
    {
        return placedObects.Where(x => x.Value.guid == guid).First().Value;
    }

    public bool IsValid(Vector3Int position, Vector2Int size)
    {
        for (int i = 0; i < size.x; i++)
        {
            for (int j = 0; j < size.y; j++)
            {
                Vector3Int tilePos = position + new Vector3Int(i, 0, j);
                int areaNumber = tilePos.GetAreaNumber();
                if (placedObects.ContainsKey(tilePos) || !SaveLoadManager.Data.AreaAuthority[areaNumber])
                    return false;
                bool isValidNumber = SaveLoadManager.Data.AreaAuthority.ContainsKey(areaNumber);
                if (isValidNumber && !SaveLoadManager.Data.AreaAuthority[areaNumber])
                {
                    return false;
                }
            }
        }
        return true;
    }

    public bool HasAuthority(Vector3Int position)
    {
        int areaNumber = position.GetAreaNumber();
        bool isValidNumber = SaveLoadManager.Data.AreaAuthority.ContainsKey(areaNumber);
        if (isValidNumber)
        {
            return SaveLoadManager.Data.AreaAuthority[areaNumber];
        }
        return false;
    }
}