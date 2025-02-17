using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class Construction : MonoBehaviour, IBuilding, ILoadableBuilding
{
    private static readonly string expIconPath = "Sprites/Icons/Icon_Level";
    private static readonly string populationIconPath = "Sprites/Icons/Icon_Population";
    private static readonly string checkIconPath = "Sprites/Icons/Icon_Check";

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

    private Image completeIcon;

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

    private void Update()
    {   
        // if(completeIcon is not null)
        //     completeIcon.transform.position = Camera.main.WorldToScreenPoint(transform.position + Vector3.up);
    }

    public void SetBuildingInfo(int buildingDataId)
    {
        this.buildingDataId = buildingDataId;
        startTime = DateTime.Now;
        finishTime = DateTime.Now + TimeSpan.FromSeconds(buildingDatabase.Get(buildingDataId).productionTime); 
    }
    
    public void OnTouch()
    {
        if (isCompleted)
        {
            int guid = gridData.GetGuid(transform.position.ToVector3Int());
            gridData.ChangeObject(guid, buildingDataId);
            objectPlacer.ChangeObject(guid, buildingDatabase.Get(buildingDataId).prefab);
            BuildingData buildingData = buildingDatabase.Get(buildingDataId);
            SaveLoadManager.Data.Exp += buildingData.exp;
            SaveLoadManager.Data.Population += buildingData.population;
            
            DefaultUI defaultUI = uiManager.GetPanel(MainSceneUiIds.Default).GetComponent<DefaultUI>();
            if (buildingData.exp != 0)
            {
                var expEndPosition = defaultUI.levelImage.position;
                var expSprite = Resources.Load<Sprite>(expIconPath);
                uiManager.iconAnimator.MoveFromWorldToUI(transform.position + Vector3.up, expEndPosition, expSprite, 0.3f);
            }
            if (buildingData.population != 0)
            {
                var populationEndPosition = defaultUI.populationImage.position;
                var populationSprite = Resources.Load<Sprite>(populationIconPath);
                uiManager.iconAnimator.MoveFromWorldToUI(transform.position + Vector3.up, populationEndPosition, populationSprite, 0.3f);
            }

            SaveLoadManager.Save();
            
            uiManager.iconAnimator.DisablePopupIcon(completeIcon);
            completeIcon = null;
        }
        else
        {
            timerBar.gameObject.SetActive(true);
            timerBar.SetInfo(startTime, finishTime, gameObject.transform.position);
        }
    }

    private void OnComplete()
    {
        Sprite sprite = Resources.Load<Sprite>(checkIconPath);
        completeIcon = uiManager.iconAnimator.PopupIconOnBuildingPos(sprite, this.transform.position + Vector3.up);
        isCompleted = true;
    }

    private async UniTask checkCompleteTask()
    {
        await UniTask.WaitUntil(()=> DateTime.Now > finishTime);
        OnComplete();
    }



}
