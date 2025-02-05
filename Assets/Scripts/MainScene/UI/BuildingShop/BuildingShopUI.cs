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
            pair.Value.onClick.AddListener(() => OnCategoryButtonTouched(pair.Key));
        }
    }

    public void OnButtonClicked()
    {
        gameObject.SetActive(true);
        placementSystem.IsTouchable = false;
        SetBuildingPanel(BuildingTypes.Farm);
        Debug.Log(buildingDatabase);
        SaveLoadManager.Save();
    }

    private void OnCategoryButtonTouched(BuildingTypes type)
    {
        SetBuildingPanel(type);
    }

    private void SetBuildingPanel(BuildingTypes type)
    {
        ClearBuildingPanel();
        foreach (var building in buildingDatabase.Dictionary)
        {
            if (building.Value.buildingType != type)
                continue;
            var button = buttonPool.GetFromPool();
            button.gameObject.SetActive(true);
            button.GetComponent<Button>().onClick.AddListener(
                () => OnBudilngPanelTouched(building.Key)
            );
            bool isAuthorized = SaveLoadManager.Data.Gold >= building.Value.cost &&
                                SaveLoadManager.Data.Level >= building.Value.level;
            button.GetComponent<Button>().interactable = isAuthorized;
            button.Init(building.Key, building.Value, isAuthorized);
            button.transform.SetParent(contents);
        }
    }

    private void OnBudilngPanelTouched(int id)
    {
        placementSystem.StartPlacement(id);
        StopUI();
    }

    private void ClearBuildingPanel()
    {
        var panels = contents.GetComponentsInChildren<BuildingPanelHandler>().ToList();
        foreach (var panel in panels)
        {
            buttonPool.ReturnToPool(panel);
        }
    }

    public void OnCloseButtonTouched()
    {
        placementSystem.IsTouchable = true;
        StopUI();
    }
    
    private void StopUI()
    {
        ClearBuildingPanel();
        gameObject.SetActive(false);
    }
}