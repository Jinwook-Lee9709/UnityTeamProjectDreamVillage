using System.Linq;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using AYellowpaper.SerializedCollections;
using UnityEngine;
using UnityEngine.UI;

public class BuildingShopUI : MonoBehaviour
{
    //References
    [SerializeField] private PlacementSystem placementSystem;
    
    //Database
    [SerializeField] private BuildingDatabaseSO buildingDatabase;

    //UiObjects
    [SerializeField] private GameObject categoryPanel;
    [SerializeField] private GameObject buildingPanel;
    [SerializeField] private SerializedDictionary<BuildingTypes, Button> categoryButtons;

    //Transform
    [SerializeField] private Transform poolParent;
    [SerializeField] private Transform contents;

    //Prefab
    [SerializeField] private BuildingPanelHandler buildingPanelPrefab;

    private ObjectPool<BuildingPanelHandler> buttonPool;

    public void Awake()
    {
        buttonPool = new ObjectPool<BuildingPanelHandler>(buildingPanelPrefab, poolParent);
        foreach (var pair in categoryButtons)
        {
            pair.Value.onClick.AddListener(() => OnCategoryPanelTouched(pair.Key));
        }
    }

    public void OnButtonClicked()
    {   
        gameObject.SetActive(true);
        categoryPanel.SetActive(true);
        placementSystem.IsTouchable = false;
    }

    private void OnCategoryPanelTouched(BuildingTypes type)
    {
        categoryPanel.SetActive(false);
        buildingPanel.SetActive(true);

        foreach (var building in buildingDatabase.Dictionary)
        {
            var button = buttonPool.GetFromPool();
            button.gameObject.SetActive(true);
            button.GetComponent<Button>().onClick.AddListener(
                () => OnBudilngPanelTouched(building.Key)
            );
            bool isAuthorized = SaveLoadManager.Data.Gold >= building.Value.cost;
            button.GetComponent<Button>().interactable = isAuthorized;
            button.Init(building.Key, building.Value);
            button.transform.SetParent(contents);
            
        }

        // switch (type)
        // {
        //     case BuildingTypes.Farm:
        //         
        //         break;
        //     case BuildingTypes.Factory:
        //         break;
        //     case BuildingTypes.Decoration:
        //         break;
        //     default:
        //         throw new ArgumentOutOfRangeException(nameof(type), type, null);
        // }
    }

    private void OnBudilngPanelTouched(int id)
    {
        placementSystem.StartPlacement(id);
        StopUI();
    }

    private void StopUI()
    {
        var panels = contents.GetComponentsInChildren<BuildingPanelHandler>().ToList();
        foreach (var panel in panels)
        {
            buttonPool.ReturnToPool(panel);
        }
        categoryPanel.SetActive(true);
        buildingPanel.SetActive(false);
        gameObject.SetActive(false);
    }
}