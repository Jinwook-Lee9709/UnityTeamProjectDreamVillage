using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridData : MonoBehaviour
{
    Dictionary<Vector2Int,PlacementData> placedObects = new Dictionary<Vector2Int,PlacementData>();

    public void AddObject(int guid, Vector2Int position, Vector2Int size)
    {
        int id = System.Guid.NewGuid().GetHashCode();
        
    }

    public bool IsValid(Vector2Int position, Vector2Int size)
    {
        for (int i = 0; i < size.x; i++)
        {
            for (int j = 0; j < size.y; j++)
            {
                if (placedObects.ContainsKey(position + new Vector2Int(i, j)))
                    return false;
            }
        }
        return true;
    }
}
