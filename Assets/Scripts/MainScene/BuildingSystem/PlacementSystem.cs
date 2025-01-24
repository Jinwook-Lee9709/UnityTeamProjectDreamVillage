
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlacementSystem : MonoBehaviour
{
   
    [SerializeField] private GameObject panel;
    [SerializeField] private Button checkButton;
    [SerializeField] private Button rotateButton;
    [SerializeField] private Button removeButton;
    [SerializeField] private Button cancelButton;
    [SerializeField] private Grid grid; 
    [SerializeField] private ObjectPlacer objectPlacer;
    [SerializeField] private BuildingDatabaseSO buildingDatabase;
    [SerializeField] private PreviewSystem previewSystem;

    public Grid Grid => grid;
    public GridData GridInfo => gridData;
    
    public bool IsTouchable { get; set; } = true;
    
    private GridData gridData;
    private IBuildingState buildingState;
    
    private Vector3Int lastDetectedPosition = Vector3Int.zero;

    private void Start()
    {
        gridData = new GridData();
        panel.SetActive(false);
    }

    public void StartPlacement(int id)
    {
        StopPlacement();
        panel.SetActive(true);
        removeButton.gameObject.SetActive(false);
        buildingState = new PlaceState(
            id,
            grid,
            objectPlacer,
            buildingDatabase,
            gridData,
            previewSystem
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
        panel.SetActive(true);
        removeButton.gameObject.SetActive(true);
        buildingState = new ModifyState(
            guid,
            grid,
            objectPlacer,
            buildingDatabase,
            gridData,
            previewSystem
        );
        checkButton.onClick.AddListener(() =>
        {
            if (OnAction())
            {
                StopPlacement();
            }
        });
        rotateButton.onClick.AddListener(OnRotation);
        removeButton.onClick.AddListener(()=>
        {
            OnRemove();
            StopPlacement();
        });
        cancelButton.onClick.AddListener(StopPlacement);
    }
    

    public void StopPlacement()
    {
        buildingState?.EndState();
        buildingState = null;
        checkButton.onClick.RemoveAllListeners();
        rotateButton.onClick.RemoveAllListeners();
        removeButton.onClick.RemoveAllListeners();
        cancelButton.onClick.RemoveAllListeners();
        panel.SetActive(false);
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
        if (MultiTouchManager.Instance.LongPress && buildingState == null && IsTouchable)
        {
            var touchedTilePos = grid.WorldToCell(InputManager.Instance.TouchPositionToPlane());
            if (!gridData.IsValid(touchedTilePos, new Vector2Int(1, 1)))
            {
                StartModify(gridData.GetGuid(touchedTilePos));
            }
        }
        
        if (buildingState == null)
            return;
        buildingState.UpdateState();
        
    }

    public Dictionary<int, GameObject> GetBuildingList()
    {
        return objectPlacer.ObjectDictionary;
    }
}
