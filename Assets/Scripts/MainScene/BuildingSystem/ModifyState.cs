using UnityEngine;

public class ModifyState : IBuildingState
{
    private int buildingDataId;
    private int guid;
    private Grid grid;
    private ObjectPlacer objectPlacer;
    private BuildingDatabaseSO buildingDatabase;
    private GridData gridData;
    private BuildingData currentBuildingData;
    private PreviewSystem previewSystem;
    private GameObject originalObject;
    private CameraManager cameraManager;
    

    private float fingerId = -1;
    private Vector3 prevTouchPos;
    // private bool isMoved = false;

    public ModifyState(int guid,
        Grid grid,
        ObjectPlacer objectPlacer,
        BuildingDatabaseSO buildingDatabase,
        GridData gridData,
        PreviewSystem previewSystem,
        CameraManager cameraManager)
    {
        this.guid = guid;
        this.grid = grid;
        this.objectPlacer = objectPlacer;
        this.buildingDatabase = buildingDatabase;
        this.gridData = gridData;
        this.previewSystem = previewSystem;
        this.cameraManager = cameraManager;
        originalObject = objectPlacer.GetObject(guid);
        buildingDataId = gridData.GetBuildingDataId(guid);
        currentBuildingData = buildingDatabase.Get(buildingDataId);
        previewSystem.enabled = true;
        bool isValid = gridData.IsValid(originalObject.transform.position.ToVector3Int(), currentBuildingData.size);
        previewSystem.ShowPlacementPreview(currentBuildingData.prefab, originalObject.transform.position, isValid);
        if (gridData.GetIsFliped(guid))
        {
            OnRotation();
        }
        originalObject.SetActive(false);
    }

    public bool OnAction()
    {
        var gridPosition = GetCurrentPreviewValidity(out var validity);
        if (validity == false)
            return false;
        originalObject.SetActive(true);
        objectPlacer.MoveObject(guid, gridPosition, previewSystem.IsFlip);
        int buildingDataId = gridData.GetBuildingDataId(guid);
        gridData.RemoveObject(guid);
        gridData.AddObject(guid, buildingDataId, gridPosition, currentBuildingData, previewSystem.IsFlip);
        return true;
    }

    private Vector3Int GetCurrentPreviewValidity(out bool validity)
    {
        Vector3 previewPosition = previewSystem.currentPreviewPosition;
        Vector3Int gridPosition = grid.WorldToCell(previewPosition);
        Vector2Int flipedSize = new Vector2Int(currentBuildingData.size.y,currentBuildingData.size.x);
        validity = gridData.IsValid(gridPosition, previewSystem.IsFlip? flipedSize : currentBuildingData.size);
        return gridPosition;
    }

    public void OnRotation()
    {
        previewSystem.RotatePreview();
    }

    public void OnRemove()
    {
        gridData.RemoveObject(guid);
        objectPlacer.RemoveObject(guid);
    }

    public void UpdateState()
    {
        CheckIsPreviewTouched();
        CheckIsTouchEnded();
        if (fingerId != -1)
        {
            Vector3 touchedPos = InputManager.Instance.Vector2PositionToPlane(Input.GetTouch(0).position);
            Vector3 touchedTile = grid.WorldToCell(touchedPos);
            if (touchedTile != prevTouchPos)
            {
                previewSystem.MovePreviewObject(touchedTile);
                GetCurrentPreviewValidity(out var validity);
                previewSystem.UpdatePreview(validity);
                prevTouchPos = touchedTile;
            }
        }
    }

    private void CheckIsTouchEnded()
    {
        if (Input.touches.Length != 1 && fingerId != -1)
            fingerId = -1;
        for (int i = 0; i < Input.touchCount; i++)
        {
            Touch touch = Input.GetTouch(i);

            if (fingerId != -1 && (touch.phase == TouchPhase.Canceled || touch.phase == TouchPhase.Ended))
            {
                fingerId = -1;
                cameraManager.ScreenEdgeMove = false;
                cameraManager.DragMove = true;
            }
        }
    }

    private void CheckIsPreviewTouched()
    {
        if (Input.touches.Length == 1 && fingerId == -1)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                Vector3 touchPos = grid.WorldToCell(InputManager.Instance.Vector2PositionToPlane(touch.position));
                if (touchPos.x == previewSystem.currentPreviewPosition.x &&
                    touchPos.z == previewSystem.currentPreviewPosition.z)
                {
                    fingerId = touch.fingerId;
                    cameraManager.ScreenEdgeMove = true;
                    cameraManager.DragMove = false;
                }
            }
        }
    }

    public void EndState()
    {
        previewSystem.EndPlacementPreview();
        originalObject.SetActive(true);
        previewSystem.enabled = false;
    }
}