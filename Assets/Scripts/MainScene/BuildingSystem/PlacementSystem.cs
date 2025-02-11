using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlacementSystem : MonoBehaviour
{
    //UI References
    [SerializeField] private UiManager uiManager;
    [SerializeField] private GameObject panel;
    [SerializeField] private Button checkButton;
    [SerializeField] private Button rotateButton;
    [SerializeField] private Button removeButton;
    [SerializeField] private Button cancelButton;
    //PlacementSystemElements
    [SerializeField] private Grid grid;
    [SerializeField] private ObjectPlacer objectPlacer;
    [SerializeField] private BuildingDatabaseSO buildingDatabase;
    [SerializeField] private PreviewSystem previewSystem;
    //References
    [SerializeField] private CameraManager cameraManager;
    [SerializeField] private AreaManager areaManager;

    public Grid Grid => grid;
    public GridData GridInfo => gridData;
    
    public ObjectPlacer ObjectPlacer => objectPlacer;

    private bool isTouchable = true;

    public bool IsTouchable
    {
        get
        {
            return isTouchable;
        }
        set
        {
            if (isTouchable != value)
            {
                isTouchable = value;
                OnTouchableChanged?.Invoke(isTouchable);
            }
        }
    } 

    private GridData gridData;
    private IBuildingState buildingState;

    private Vector3Int lastDetectedPosition = Vector3Int.zero;

    public event Action<bool> OnTouchableChanged;
    
    private void Awake()
    {
        if(gridData == null)
            gridData = new GridData();
        panel.SetActive(false);
    }

    private void Start()
    {
        SaveLoadManager.OnBeforeSave += SaveBuildings;
        LoadBuildings();
    }

    public void StartPlacement(int id)
    {
        StopPlacement();
        uiManager.SetDefaultUiInteract(false);
        IsTouchable = false;
        panel.SetActive(true);
        removeButton.gameObject.SetActive(false);
        buildingState = new PlaceState(
            id,
            grid,
            objectPlacer,
            buildingDatabase,
            gridData,
            previewSystem,
            cameraManager
        );
        checkButton.onClick.AddListener(() =>
        {
            if (OnAction())
            {
                StopPlacement();
            }
        });
        rotateButton.onClick.AddListener(OnRotation);
        cancelButton.onClick.AddListener(StopPlacement);
    }

    public void StartModify(int guid)
    {
        StopPlacement();
        uiManager.SetDefaultUiInteract(false);
        IsTouchable = false;
        panel.SetActive(true);
        removeButton.gameObject.SetActive(true);
        buildingState = new ModifyState(
            guid,
            grid,
            objectPlacer,
            buildingDatabase,
            gridData,
            previewSystem,
            cameraManager
        );
        checkButton.onClick.AddListener(() =>
        {
            if (OnAction())
            {
                StopPlacement();
            }
        });
        rotateButton.onClick.AddListener(OnRotation);
        removeButton.onClick.AddListener(() =>
        {
            OnRemove();
            StopPlacement();
        });
        cancelButton.onClick.AddListener(StopPlacement);
    }


    public void StopPlacement()
    {
        uiManager.SetDefaultUiInteract(true);
        buildingState?.EndState();
        buildingState = null;
        checkButton.onClick.RemoveAllListeners();
        rotateButton.onClick.RemoveAllListeners();
        removeButton.onClick.RemoveAllListeners();
        cancelButton.onClick.RemoveAllListeners();
        IsTouchable = true;
        panel.SetActive(false);

        cameraManager.ScreenEdgeMove = false;
    }

    public bool OnAction()
    {
        return buildingState.OnAction();
    }

    public void OnRotation()
    {
        buildingState.OnRotation();
    }

    public void OnRemove()
    {
        buildingState.OnRemove();
    }

    public bool IsBuildingExist(Vector3Int gridPosition, int index)
    {
        return !gridData.IsValid(gridPosition, buildingDatabase.Get(index).size);
    }

    private void Update()
    {
        if (MultiTouchManager.Instance.LongPress && buildingState == null && IsTouchable &&
            !EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
        {
            var touchedTilePos = grid.WorldToCell(InputManager.Instance.TouchPositionToPlane());
            if (!gridData.IsValid(touchedTilePos, new Vector2Int(1, 1)) &&
                gridData.HasAuthority(touchedTilePos))
            {
                StartModify(gridData.GetGuid(touchedTilePos));
            }
            else if (!gridData.HasAuthority(touchedTilePos))
            {
                areaManager.OnTilePressed(touchedTilePos);
            }
        }
        if (buildingState == null)
            return;
        buildingState.UpdateState();
    }
    
    public async UniTask DelayedActivate(float duration = 0.01f)
    {
        await Task.Delay((int)(duration * 1000));
        IsTouchable = true;
    }

    public Dictionary<int, GameObject> GetBuildingList()
    {
        return objectPlacer.ObjectDictionary;
    }

    private void SaveBuildings()
    {
        SaveLoadManager.Data.buildings.Clear();
        foreach (var pair in objectPlacer.ObjectDictionary)
        {
            BuildingSaveData saveData = new();
            PlacementData placementData = gridData.GetPlacementData(pair.Key);
            saveData.position = placementData.pivotPoint;
            saveData.buildingId = placementData.buildingDataId;
            saveData.isFlip = placementData.isFlip;
            saveData.task = BuildingSaveDataManager.MakeTaskData(pair.Value.GetComponent<IBuilding>());
            SaveLoadManager.Data.buildings.Add(saveData);
        }
    }

    private void LoadBuildings()
    {
        foreach (var data in SaveLoadManager.Data.buildings)
        {
            int guid = Guid.NewGuid().GetHashCode();
            gridData.AddObject(guid, data.buildingId, data.position,
                buildingDatabase.Get(data.buildingId), data.isFlip, false);
            GameObject obj = objectPlacer.PlaceObject(guid, data.buildingId, data.position, data.isFlip, false);
            if (data.task != null)
            {
                obj.GetComponent<ILoadableBuilding>().Load(data.task);
            }
        }
    }
}