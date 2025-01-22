using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPlacer : MonoBehaviour
{
    public GameObject parents;
    public Dictionary<int, GameObject> Objects = new Dictionary<int, GameObject>();

    public void PlaceObject(int guid, GameObject prefab, Vector3 position, bool isFlip)
    {
        GameObject obj = Instantiate(prefab, position, Quaternion.identity);
        obj.transform.parent = parents.transform;
        if (isFlip)
        {
            obj.transform.GetChild(0).transform.Rotate(Vector3.up, 90f);
        }
        Objects.Add(guid, obj);
    }

    public void RemoveObject(int guid)
    {
        if (Objects.ContainsKey(guid))
        {
            Destroy(Objects[guid]);
            Objects.Remove(guid);
        }
        else
        {
            Debug.LogError("Object Remove Failed");
        }
    }
}
