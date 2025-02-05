using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public enum FarmState
{
    None,
    Growing,
    Completed
}

public enum InteractionState
{
    None,
    Planting,
    Harvesting,
}

public class Farm : MonoBehaviour, IBuilding
{
    private static readonly string sicklePath = "Sprites/Icons/Harvest_Icon_Sickle";
    private static readonly string iconPath = "Sprites/Icons/Item_Icon_{0}";
    private static readonly string growPrefabPath = "Prefabs/Crops/Crop_Prefab_{0}_Grow";
    private static readonly string completePrefabPath = "Prefabs/Crops/Crop_Prefab_{0}_Complete";
    
    [SerializeField] private GameObject[] cropsPivot;
    [SerializeField] private int selectedCropIndex;
    [SerializeField] private CropRecipeDatabaseSo cropRecipeDatabase;
    [SerializeField] private GameObject imagePrefab;
    
    private FarmState farmState = FarmState.None; 
    private InteractionState interactionState = InteractionState.None;
    
    //References
    private GameManager gameManager;
    private UiManager uiManager;
    private GameObject panel;
    private GameObject uiContent;
    private FarmingUI farmingUI;
    private RectTransform rectTransform;
    private GameObject cursor;
    private Grid grid;
    private GridData gridInfo;
    private TimerBarUI timerBar;
    private Dictionary<int, GameObject> buildings;
    
    //ForPlantUI
    private int currentCursorCropId = -1;
    private bool isTouching = false;
    private int fingerId = -1;
    private Vector3Int prevCursorPosition = Vector3Int.zero;
    private bool isEverPlanted = false;
    
    //ForHarvest
    private DateTime? plantedTime = null;
    private DateTime? finishTime = null;
    private int plantedCropId = -1;
    
    public void OnTouch()
    {
        switch (farmState)
        {
            case FarmState.None:
                StartPlantMode();
                break;
            case FarmState.Growing:
                ShowRemainTime();
                break;
            case FarmState.Completed:
                StartHarvestMode();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        
    }
    
    private void StartPlantMode()
    {
        if (panel.activeSelf == false)
        {
            farmingUI.ShowPlantUI(OnCropIconTouched);
            if (rectTransform == null)
            {
                rectTransform = panel.transform.GetComponentsInChildren<RectTransform>().FirstOrDefault(t => t.name == "Viewport");     
            }
            gameManager.PlacementSystem.IsTouchable = false;
            uiManager.SetDefaultUiInteract(false);
            interactionState = InteractionState.Planting;
        }
    }
    private void ShowRemainTime()
    {
        timerBar.gameObject.SetActive(true);
        if(plantedTime is not null && finishTime is not null)
            timerBar.SetInfo(plantedTime.Value, finishTime.Value, transform.position);
    }


    private void StartHarvestMode()
    {
        if (panel.activeSelf == false)
        {
            farmingUI.ShowHarvestUI(OnSickelIconTouched);
            if (rectTransform == null)
            {
                rectTransform = panel.transform.GetComponentsInChildren<RectTransform>().FirstOrDefault(t => t.name == "Viewport");     
            }
            gameManager.PlacementSystem.IsTouchable = false;
            uiManager.SetDefaultUiInteract(false);
            interactionState = InteractionState.Harvesting;
        }
    }

    public void Init(GameManager gameManager, UiManager uiManager, bool isFirst)
    {
        this.uiManager = uiManager;
        panel = uiManager.GetPanel(MainSceneUiIds.Farming);
        WorldUI worldUI = uiManager.GetPanel(MainSceneUiIds.World).GetComponent<WorldUI>();
        timerBar = worldUI.timerBar;
        
        farmingUI = panel.GetComponent<FarmingUI>();
        uiContent = panel.transform.GetComponentsInChildren<Transform>().FirstOrDefault(t => t.name == "Content").gameObject;
        rectTransform = panel.transform.GetComponentsInChildren<RectTransform>().FirstOrDefault(t => t.name == "Viewport");
        this.gameManager = gameManager;
        grid = gameManager.PlacementSystem.Grid;
        gridInfo = gameManager.PlacementSystem.GridInfo;
        buildings = gameManager.PlacementSystem.GetBuildingList();
    }

    public void OnCropIconTouched(int id)
    {
        if (Input.touches.Length == 1)
        {
            currentCursorCropId = id;
            fingerId = Input.GetTouch(0).fingerId;
            isTouching = true;
            cursor = Instantiate(imagePrefab, panel.transform.parent);
            cursor.GetComponent<Image>().sprite = Resources.Load<Sprite>(string.Format(iconPath, currentCursorCropId));
        }
    }

    public void OnSickelIconTouched()
    {
        if (Input.touches.Length == 1)
        {
            fingerId = Input.GetTouch(0).fingerId;
            isTouching = true;
            cursor = Instantiate(imagePrefab, panel.transform.parent);
            cursor.GetComponent<Image>().sprite = Resources.Load<Sprite>(sicklePath);
        }
    }

    private void Update()
    {

        if (panel is null)
            return;
        switch (interactionState)
        {
            case InteractionState.None:
                break;
            case InteractionState.Planting:
                HandleTouchEventAtPlant();
                break;
            case InteractionState.Harvesting:
                HandleTouchEventAtHarvest();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        CheckCompleteTime();
    }

    private void CheckCompleteTime()
    {
        if (finishTime != null && finishTime < DateTime.Now)
        {
            finishTime = null;
            farmState = FarmState.Completed;
            foreach (GameObject crop in cropsPivot)
            {
                Destroy(crop.transform.GetChild(0).gameObject);
                Instantiate(Resources.Load<GameObject>(String.Format(completePrefabPath, plantedCropId)), crop.transform);
            }
        }
    }

    private void HandleTouchEventAtPlant()
    {
        if (panel.activeSelf != false &&
            Input.touches.Length != 0 && 
            Input.touches[0].phase == TouchPhase.Began && 
            !RectTransformUtility.RectangleContainsScreenPoint(rectTransform, Input.touches[0].position))
        {
            EndPlantState();
            return;
        }
        foreach(var touch in Input.touches)
        {
            if (touch.fingerId == fingerId)
            {
                var currentTouch = touch;
                if (currentTouch.phase == TouchPhase.Canceled || currentTouch.phase == TouchPhase.Ended)
                {
                    Destroy(cursor);
                    isTouching = false;
                    fingerId = -1;
                    currentCursorCropId = -1;
                    farmingUI.SetScrollRectActivation(true);
                    if (isEverPlanted)
                    {
                        EndPlantState();
                    }
                }
                if (isTouching)
                {
                    cursor.transform.position = currentTouch.position;
                    if (panel.activeSelf == true &&
                        RectTransformUtility.RectangleContainsScreenPoint(rectTransform, currentTouch.position)
                        && isEverPlanted)
                    {
                        panel.SetActive(false);
                    }
                    Vector3 touchedPlanePos = InputManager.Instance.Vector2PositionToPlane(currentTouch.position);
                    Vector3Int touchedTilePos = grid.WorldToCell(touchedPlanePos);
                    if (prevCursorPosition != touchedTilePos)
                    {
                        prevCursorPosition = touchedTilePos;
                        PlantCrop(touchedTilePos);
                    }
                }
            }
        }
    }

    private void PlantCrop(Vector3Int touchedTilePos)
    {
        int guid = gridInfo.GetGuid(touchedTilePos);
        if (guid != -1)
        {
            if (buildings.ContainsKey(guid))
            {
                Farm farm = buildings[guid].GetComponent<Farm>();
                if (farm != null && farm.farmState == FarmState.None)
                {
                    var cropData = cropRecipeDatabase.Get(currentCursorCropId);
                    int cost = cropData.necessaryCost;
                    if ( cost <= SaveLoadManager.Data.Gold)
                    {
                        SaveLoadManager.Data.Gold -= cost;
                        isEverPlanted = true;
                        farm.Plant(currentCursorCropId);
                    }
                }
            }
        }
    }

    private void HandleTouchEventAtHarvest()
    {
         if (panel.activeSelf != false &&
            Input.touches.Length != 0 && 
            Input.touches[0].phase == TouchPhase.Began && 
            !RectTransformUtility.RectangleContainsScreenPoint(rectTransform, Input.touches[0].position))
        {
            EndHarvestState();
            return;
        }
        foreach(var touch in Input.touches)
        {
            if (touch.fingerId == fingerId)
            {
                var currentTouch = touch;
                if (currentTouch.phase == TouchPhase.Canceled || currentTouch.phase == TouchPhase.Ended)
                {
                    Destroy(cursor);
                    isTouching = false;
                    fingerId = -1;
                    if (isEverPlanted)
                    {
                        EndHarvestState();
                    }
                }
                if (isTouching)
                {
                    cursor.transform.position = currentTouch.position;
                    if (panel.activeSelf == true &&
                        RectTransformUtility.RectangleContainsScreenPoint(rectTransform, currentTouch.position)
                        && isEverPlanted)
                    {
                        panel.SetActive(false);
                    }
                    Vector3 touchedPlanePos = InputManager.Instance.Vector2PositionToPlane(currentTouch.position);
                    Vector3Int touchedTilePos = grid.WorldToCell(touchedPlanePos);
                    if (prevCursorPosition != touchedTilePos)
                    {
                        prevCursorPosition = touchedTilePos;
                        HarvestCrop(touchedTilePos);
                    }
                }
            }
        }
    }

    private void HarvestCrop(Vector3Int touchedTilePos)
    {
        int guid = gridInfo.GetGuid(touchedTilePos);
        if (guid != -1)
        {
            if (buildings.ContainsKey(guid))
            {
                Farm farm = buildings[guid].GetComponent<Farm>();
                if (farm != null && farm.farmState == FarmState.Completed)
                {
                    farm.Harvest();
                }
            }
        }
    }


    public void Plant(int id)
    {
        if (plantedTime == null)
        {
            for (int i = 0; i < cropsPivot.Length; i++)
            {
                var crop = Instantiate(Resources.Load<GameObject>(String.Format(growPrefabPath, id)), cropsPivot[i].transform);
                plantedTime = DateTime.Now;
                finishTime = plantedTime + TimeSpan.FromSeconds(cropRecipeDatabase.Get(id).productionTime);
                plantedCropId = id;
                farmState = FarmState.Growing;
            }
        }
    }

    public void Harvest()
    {
        farmState = FarmState.None;
        plantedTime = null;
        finishTime = null;
        var cropData = cropRecipeDatabase.Get(plantedCropId);
        SaveLoadManager.Data.inventory.AddItem(plantedCropId, cropData.productCount);
        SaveLoadManager.Data.Exp += cropData.exp;
        SaveLoadManager.Save();
        plantedCropId = -1;
        foreach (GameObject crop in cropsPivot)
        {
            Destroy(crop.transform.GetChild(0).gameObject);
        }
    }
    private void EndState()
    {
        Destroy(cursor);
        gameManager.PlacementSystem.IsTouchable = true;
        uiManager.SetDefaultUiInteract(true);
        farmingUI.StopFarmingUI();
        panel.SetActive(false);
        farmingUI.SetScrollRectActivation(true);
        interactionState = InteractionState.None;
    }

    private void EndPlantState()
    {
        isEverPlanted = false;
        currentCursorCropId = -1;
        EndState();
    }

    private void EndHarvestState()
    {
        EndState();
    }
}
