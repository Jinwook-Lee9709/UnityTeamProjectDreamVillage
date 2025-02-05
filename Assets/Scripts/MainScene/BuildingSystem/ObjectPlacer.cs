using System.Collections.Generic;
using UnityEngine;

public class ObjectPlacer : MonoBehaviour
{
    [SerializeField] private BuildingDatabaseSO buildingDatabase;
    
    public GameObject parents;
    public GameManager gameManager;
    public UiManager uiManaqger;
    private Dictionary<int, GameObject> Objects = new();

    public Dictionary<int, GameObject> ObjectDictionary { get => Objects; }
    
    
    public GameObject PlaceObject(int guid, int buildingId, Vector3 position, bool isFlip)
    {
        var buildingData = buildingDatabase.Get(buildingId);
        bool isNeedTime = buildingData.productionTime != 0;
        GameObject prefab = null;
        GameObject obj = null;
        if (isNeedTime)
        {
            prefab = buildingDatabase.Get(Variables.constructionBuildingId).prefab;
            obj = Instantiate(prefab , position, Quaternion.identity);
            var construction = obj.GetComponent<Construction>();
            construction.SetBuildingInfo(buildingId);
        }
        else
        {
            prefab = buildingDatabase.Get(buildingId).prefab;
            obj = Instantiate(prefab , position, Quaternion.identity);
        }
        obj.transform.parent = parents.transform;
        obj.GetComponent<IBuilding>().Init(gameManager, uiManaqger);
        if (isFlip)
        {
            obj.transform.GetChild(0).transform.Rotate(Vector3.up, 90f);
        }
        Objects.Add(guid, obj);
        return obj;
    }

    public void ChangeObject(int guid, GameObject prefab)
    {
        var oldObj = Objects[guid];
        GameObject obj = Instantiate(prefab, oldObj.transform.position, oldObj.transform.rotation);
        obj.transform.parent = parents.transform;
        obj.GetComponent<IBuilding>().Init(gameManager, uiManaqger);
        
        Objects.Remove(guid);
        Destroy(oldObj);
        Objects.Add(guid, obj);
    }

    public void MoveObject(int guid, Vector3 position, bool isFlip)
    {
        var obj = Objects[guid];
        obj.transform.position = position;
        obj.transform.GetChild(0).rotation = isFlip ? Quaternion.Euler(Vector3.up * 90f) : Quaternion.Euler(Vector3.zero);

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

    public GameObject GetObject(int guid)
    {
        return Objects[guid];
    }
}
