using System;
using UnityEngine;

public class PlaceState : IBuildingState
{
    private int id;
    private Grid grid;
    private ObjectPlacer objectPlacer;
    private BuildingDatabaseSO buildingDatabase;
    private GridData gridData;
    private BuildingData currentBuildingData;
    private PreviewSystem previewSystem;

    private float fingerId = -1;
    private Vector3 prevTouchPos;

    public PlaceState(int id,
        Grid grid,
        ObjectPlacer objectPlacer,
        BuildingDatabaseSO buildingDatabase,
        GridData gridData,
        PreviewSystem previewSystem)
    {
        this.id = id;
        this.grid = grid;
        this.objectPlacer = objectPlacer;
        this.buildingDatabase = buildingDatabase;
        this.gridData = gridData;
        this.previewSystem = previewSystem;
        currentBuildingData = buildingDatabase.Get(id);
        previewSystem.enabled = true;
        previewSystem.ShowPlacementPreview(currentBuildingData.prefab,
        grid.WorldToCell(InputManager.Instance.CenterPositionToPlane()));
    }

    public bool OnAction()
    {
        var gridPosition = GetCurrentPreviewValidity(out var validity);
        if (validity == false)
            return false;

        int guid = Guid.NewGuid().GetHashCode();
        gridData.AddObject(id, guid, gridPosition, currentBuildingData, previewSystem.IsFlip);
        objectPlacer.PlaceObject(guid, currentBuildingData.prefab, gridPosition, previewSystem.IsFlip);
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
                }
            }
        }
    }

    public void EndState()
    {
        previewSystem.EndPlacementPreview();
        previewSystem.enabled = false;
    }
}