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
    [SerializeField] private Image backgroundImage;
    [SerializeField] private GameObject mainPanel;
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
        mainPanel.transform.localScale = Vector3.one;
        SetBuildingPanel(BuildingTypes.Farm);
        DotAnimator.DissolveInAnimation(backgroundImage, alpha:0.7f);
        DotAnimator.PopupAnimation(mainPanel);
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
            button.transform.localScale = Vector3.one;
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
        DotAnimator.DissolveOutAnimation(backgroundImage, onComplete:() => gameObject.SetActive(false));
        DotAnimator.CloseAnimation(mainPanel, onComplete: () =>
        {
            ClearBuildingPanel();
            gameObject.SetActive(false);
        });
    }
}