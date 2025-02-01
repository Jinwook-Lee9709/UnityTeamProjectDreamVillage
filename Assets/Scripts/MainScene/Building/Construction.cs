using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class Construction : MonoBehaviour, IBuilding
{
    [SerializeField] private BuildingDatabaseSO buildingDatabase;
    
    private GameManager gameManager;
    private UiManager uiManager;
    private GridData gridData;
    private ObjectPlacer objectPlacer;

    private int buildingDataId;
    private DateTime endTime;

    private bool isCompleted;

    private void Start()
    {
        if (DateTime.Now > endTime)
        {
            OnComplete();
        }
        else
        {
            checkCompleteTask();
        }
    }

    public void SetBuildingInfo(int buildingDataId)
    {
        this.buildingDataId = buildingDataId;
        // endTime = DateTime.Now + TimeSpan.FromSeconds(buildingDatabase.Get(buildingDataId).productionTime); 
    }
    
    public void OnTouch()
    {
        if (isCompleted)
        {
            int guid = gridData.GetGuid(transform.position.ToVector3Int());
            gridData.ChangeObject(guid, buildingDataId);
            objectPlacer.ChangeObject(guid, buildingDatabase.Get(buildingDataId).prefab);
        }
        else
        {
            
        }
    }

    private void OnComplete()
    {
        isCompleted = true;
    }

    private async UniTask checkCompleteTask()
    {
        await UniTask.WaitUntil(()=> DateTime.Now > endTime);
        OnComplete();
    }
    
    public void Init(GameManager gameManager, UiManager uiManager, bool isFirst)
    {
        this.gameManager = gameManager;
        this.uiManager = uiManager;
        gridData = gameManager.PlacementSystem.GridInfo;
        objectPlacer = gameManager.PlacementSystem.ObjectPlacer;
    }
}
