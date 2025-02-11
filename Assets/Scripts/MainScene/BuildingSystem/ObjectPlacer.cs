using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ObjectPlacer : MonoBehaviour
{
    private static readonly string expIconPath = "Sprites/Icons/Icon_Level";
    private static readonly string populationIconPath = "Sprites/Icons/Icon_Population";
    
    [SerializeField] private BuildingDatabaseSO buildingDatabase;
    
    public GameObject parents;
    public GameManager gameManager;
    public UiManager uiManager;
    private Dictionary<int, GameObject> Objects = new();

    public Dictionary<int, GameObject> ObjectDictionary { get => Objects; }
    
    
    public GameObject PlaceObject(int guid, int buildingId, Vector3 position, bool isFlip, bool isFirst = true)
    {
        var buildingData = buildingDatabase.Get(buildingId);
        bool isNeedTime = buildingData.productionTime != 0;
        GameObject prefab = null;
        GameObject obj = null;
        if (isNeedTime && isFirst)
        {
            int constructionBuildingId = Variables.constructionBuildingId + 2 * (buildingData.size.x - 1) + (buildingData.size.y - 1);
            prefab = buildingDatabase.Get(constructionBuildingId).prefab;
            obj = Instantiate(prefab , position, Quaternion.identity);
            var construction = obj.GetComponent<Construction>();
            construction.SetBuildingInfo(buildingId);
        }
        else
        {
            prefab = buildingData.prefab;
            obj = Instantiate(prefab , position, Quaternion.identity);
            if (isFirst)
            {
                SaveLoadManager.Data.Population += buildingData.population;
                SaveLoadManager.Data.Exp += buildingData.exp;
                
                DefaultUI defaultUI = uiManager.GetPanel(MainSceneUiIds.Default).GetComponent<DefaultUI>();
                if (buildingData.exp != 0)
                {
                    var expEndPosition = defaultUI.levelImage.position;
                    var expSprite = Resources.Load<Sprite>(expIconPath);
                    uiManager.iconAnimator.MoveFromWorldToUI(position + Vector3.up, expEndPosition, expSprite, 0.3f);
                }
                if (buildingData.population != 0)
                {
                    var populationEndPosition = defaultUI.populationImage.position;
                    var populationSprite = Resources.Load<Sprite>(populationIconPath);
                    uiManager.iconAnimator.MoveFromWorldToUI(position + Vector3.up, populationEndPosition, populationSprite, 0.3f);
                }
            }
        }
        obj.transform.parent = parents.transform;
        obj.GetComponent<IBuilding>().Init(gameManager, uiManager);
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
        GameObject obj = Instantiate(prefab, oldObj.transform.position, Quaternion.identity);
        obj.transform.GetChild(0).rotation = oldObj.transform.GetChild(0).rotation;
        obj.transform.parent = parents.transform;
        obj.GetComponent<IBuilding>().Init(gameManager, uiManager);
        
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
