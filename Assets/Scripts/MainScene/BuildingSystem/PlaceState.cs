using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlaceState : IBuildingState
{
    private int id;
    private Grid grid;
    private ObjectPlacer objectPlacer;
    private BuildingDatabaseSO buildingDatabase;
    private GridData gridData;
    private BuildingData currentBuildingData;
    private PreviewSystem previewSystem;
    private CameraManager cameraManager;

    private float fingerId = -1;
    private Vector3 prevTouchPos;

    public PlaceState(int id,
        Grid grid,
        ObjectPlacer objectPlacer,
        BuildingDatabaseSO buildingDatabase,
        GridData gridData,
        PreviewSystem previewSystem,
        CameraManager cameraManager)
    {
        this.id = id;
        this.grid = grid;
        this.objectPlacer = objectPlacer;
        this.buildingDatabase = buildingDatabase;
        this.gridData = gridData;
        this.previewSystem = previewSystem;
        this.cameraManager = cameraManager;
        
        currentBuildingData = buildingDatabase.Get(id);
        
        var startPos = grid.WorldToCell(InputManager.Instance.CenterPositionToPlane());
        bool isValid = gridData.IsValid(startPos, currentBuildingData.size);
        previewSystem.enabled = true;
        previewSystem.ShowPlacementPreview(currentBuildingData.prefab, startPos, isValid);

        cameraManager.DragMove = false;
        cameraManager.ScreenEdgeMove = true;
    }

    public bool OnAction()
    {
        var gridPosition = GetCurrentPreviewValidity(out var validity);
        if (validity == false)
            return false;
        SaveLoadManager.Data.Gold -= buildingDatabase.Get(id).cost;
        int guid = Guid.NewGuid().GetHashCode();
        gridData.AddObject(guid, id, gridPosition, currentBuildingData, previewSystem.IsFlip);
        objectPlacer.PlaceObject(guid, id, gridPosition, previewSystem.IsFlip);
        
        return true;
    }

    private Vector3Int GetCurrentPreviewValidity(out bool validity)
    {
        Vector3 previewPosition = previewSystem.currentPreviewPosition;
        Vector3Int gridPosition = grid.WorldToCell(previewPosition);
        Vector2Int flipedSize = new Vector2Int(currentBuildingData.size.y, currentBuildingData.size.x);
        validity = gridData.IsValid(gridPosition, previewSystem.IsFlip ? flipedSize : currentBuildingData.size);
        return gridPosition;
    }

    public void OnRotation()
    {
        previewSystem.RotatePreview();
    }

    public void OnRemove()
    {
    }

    public void UpdateState()
    {
        // CheckIsPreviewTouched();
        // CheckIsTouchEnded();
        if (Input.touchCount > 0 && EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
            return;
        
        if (Input.touchCount == 1)
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

    // private void CheckIsTouchEnded()
    // {
    //     if (Input.touches.Length != 1 && fingerId != -1)
    //         fingerId = -1;
    //     for (int i = 0; i < Input.touchCount; i++)
    //     {
    //         Touch touch = Input.GetTouch(i);
    //
    //         if (fingerId != -1 && (touch.phase == TouchPhase.Canceled || touch.phase == TouchPhase.Ended))
    //         {
    //             fingerId = -1;
    //             cameraManager.ScreenEdgeMove = false;
    //             cameraManager.DragMove = true;
    //         }
    //     }
    // }
    //
    // private void CheckIsPreviewTouched()
    // {
    //     if (Input.touches.Length == 1 && fingerId == -1)
    //     {
    //         Touch touch = Input.GetTouch(0);
    //         if (touch.phase == TouchPhase.Began)
    //         {
    //             Vector3 touchPos = grid.WorldToCell(InputManager.Instance.Vector2PositionToPlane(touch.position));
    //             if (touchPos.x == previewSystem.currentPreviewPosition.x &&
    //                 touchPos.z == previewSystem.currentPreviewPosition.z)
    //             {
    //                 fingerId = touch.fingerId;
    //                 cameraManager.ScreenEdgeMove = true;
    //                 cameraManager.DragMove = false;
    //             }
    //         }
    //     }
    // }

    public void EndState()
    {
        previewSystem.EndPlacementPreview();
        cameraManager.DragMove = true;
        cameraManager.ScreenEdgeMove = false;
        previewSystem.enabled = false;
    }
}