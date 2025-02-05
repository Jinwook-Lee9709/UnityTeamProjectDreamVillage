using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class Construction : MonoBehaviour, IBuilding, ILoadableBuilding
{
    [SerializeField] private BuildingDatabaseSO buildingDatabase;
    
    private GameManager gameManager;
    private UiManager uiManager;
    private GridData gridData;
    private ObjectPlacer objectPlacer;

    private int buildingDataId;
    
    private DateTime startTime;
    private DateTime finishTime;

    private TimerBarUI timerBar;

    private bool isCompleted;

    public DateTime StartTime => startTime;
    public int BuildingDataId => buildingDataId;

    private void Start()
    {
        if (DateTime.Now > finishTime)
        {
            OnComplete();
        }
        else
        {
            checkCompleteTask();
        }
    }
    
    public void Load(BuildingTaskData buildingTaskData)
    {
        var data = buildingTaskData as ConstructionTaskData;
        buildingDataId = data.buildingDataId;
        startTime = data.startTime;
        finishTime = startTime + TimeSpan.FromSeconds(buildingDatabase.Get(buildingDataId).productionTime);
    }
    
    public void Init(GameManager gameManager, UiManager uiManager, bool isFirst)
    {
        this.gameManager = gameManager;
        this.uiManager = uiManager;
        WorldUI worldUI = uiManager.GetPanel(MainSceneUiIds.World).GetComponent<WorldUI>();
        timerBar = worldUI.timerBar;
        gridData = gameManager.PlacementSystem.GridInfo;
        objectPlacer = gameManager.PlacementSystem.ObjectPlacer;
    }

    public void SetBuildingInfo(int buildingDataId)
    {
        this.buildingDataId = buildingDataId;
        startTime = DateTime.Now;
        finishTime = DateTime.Now + TimeSpan.FromSeconds(buildingDatabase.Get(buildingDataId).productionTime); 
    }
    
    public void OnTouch()
    {
        Debug.Log(isCompleted);
        if (isCompleted)
        {
            int guid = gridData.GetGuid(transform.position.ToVector3Int());
            gridData.ChangeObject(guid, buildingDataId);
            objectPlacer.ChangeObject(guid, buildingDatabase.Get(buildingDataId).prefab);
        }
        else
        {
            timerBar.gameObject.SetActive(true);
            timerBar.SetInfo(startTime, finishTime, gameObject.transform.position);
        }
    }

    private void OnComplete()
    {
        isCompleted = true;
    }

    private async UniTask checkCompleteTask()
    {
        await UniTask.WaitUntil(()=> DateTime.Now > finishTime);
        OnComplete();
    }



}
