using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Vector3 = UnityEngine.Vector3;

public class PlacementSystem : MonoBehaviour
{
    [SerializeField] private Button checkButton;
    [SerializeField] private Button rotateButton;
    [SerializeField] private Button cancelButton;
    [SerializeField] private Grid grid;
    [SerializeField] private GridData gridData;
    [SerializeField] private ObjectPlacer objectPlacer;
    [SerializeField] private BuildingDatabaseSO buildingDatabase;
    [SerializeField] private PreviewSystem previewSystem;
    
    private IBuildingState buildingState;
    
    private Vector3Int lastDetectedPosition = Vector3Int.zero;

    private void Start()
    {
        gridData = new GridData();
    }

    public void StartPlacement(int id)
    {
        StopPlacement();
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

    public void StopPlacement()
    {
        buildingState?.EndState();
        buildingState = null;
        checkButton.onClick.RemoveAllListeners();
        rotateButton.onClick.RemoveAllListeners();
        cancelButton.onClick.RemoveAllListeners();
    }

    public bool OnAction()
    {
        return buildingState.OnAction();
    }

    public void OnRotation()
    {
        buildingState.OnRotation();
    }

    public bool IsBuildingExist(Vector3Int gridPosition, int index)
    {
        return !gridData.IsValid(gridPosition, buildingDatabase.Get(index).size);
    }
    
    private void Update()
    {
        if (buildingState == null)
            return;
        buildingState.UpdateState();
        
    }
    
    
}
