using System.Linq;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using AYellowpaper.SerializedCollections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BuildingShopUI : MonoBehaviour
{
    private static readonly int FreezeScrollCount = 3;
    //References
    [SerializeField] private PlacementSystem placementSystem;
    
    //Database
    [SerializeField] private BuildingDatabaseSO buildingDatabase;

    //UiObjects
    [SerializeField] private Image backgroundImage;
    [SerializeField] private GameObject mainPanel;
    [SerializeField] private GameObject buildingPanel;
    [SerializeField] private ScrollRect scrollRect;
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
        
        SoundManager.Instance.PlaySfxByName(AudioNames.Popup.ToString());
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

        scrollRect.enabled = contents.childCount > FreezeScrollCount;
        RectTransform contentRect = contents.GetComponent<RectTransform>();
        contentRect.anchoredPosition = new Vector2(0, contentRect.anchoredPosition.y);
        
        foreach (var pair in categoryButtons)
        {
            Color originalColor = pair.Value.GetComponent<Image>().color;
            Color.RGBToHSV(originalColor, out float h, out float s, out float v);
            v = pair.Key == type ? 1f : 0.6f; 
            
            Color newColor = Color.HSVToRGB(h, s, v);
            pair.Value.GetComponent<Image>().color = newColor;

        }
    }

    private void OnBudilngPanelTouched(int id)
    {
        placementSystem.StartPlacement(id);
        placementSystem.IsTouchable = false;
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
            SoundManager.Instance.PlaySfxByName(AudioNames.Close.ToString());
            ClearBuildingPanel();
            gameObject.SetActive(false);
        });
        
    }
}