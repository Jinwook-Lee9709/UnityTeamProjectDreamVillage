using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Experimental.Rendering.RenderGraphModule;
using Random = UnityEngine.Random;

public class Delivery : MonoBehaviour, IBuilding, ILoadableBuilding
{
    private static readonly int clientCount = 7;
    private static readonly int maxTaskCount = 4;
    private static readonly int minimumLevel = 4;
    
    [SerializeField] DeliveryDatabaseSO deliveryDatabase;
    
    private DeliveryUI panel;
    private GameManager gameManager;
    private UiManager uiManager;
    
    public void Init(GameManager gameManager, UiManager uiManager, bool IsFirst = true)
    {
        this.gameManager = gameManager;
        this.uiManager = uiManager;
        panel = this.uiManager.GetPanel(MainSceneUiIds.Delivery).GetComponent<DeliveryUI>();
    }
    public void OnTouch()
    {
        if (SaveLoadManager.Data.Level < minimumLevel)
            return;
        gameManager.PlacementSystem.IsTouchable = false;
        UpdateDeiliveryData();
        panel.Init(OnUIClosed);
        panel.gameObject.SetActive(true);
    }

    private void UpdateDeiliveryData()
    {
        DateTime lastUpdateTime = SaveLoadManager.Data.deliverySaveData.lastUpdateTime;
        DateTime now = DateTime.Now;
        DateTime today6AM = new DateTime(now.Year, now.Month, now.Day, 6, 0, 0);
        bool timeChanged = lastUpdateTime <= today6AM && today6AM < now;
        if (timeChanged || 
            SaveLoadManager.Data.deliverySaveData.deliveryList.Count == 0)
        {
            SaveLoadManager.Data.deliverySaveData.deliveryList.Clear();
            SaveLoadManager.Data.deliverySaveData.deliveryList = GetRandomTask();
            SaveLoadManager.Data.deliverySaveData.lastUpdateTime = now;

            var clinetIds = Enumerable.Range(1, clientCount + 1).OrderBy(x => Random.value).Take(maxTaskCount).ToList();
            SaveLoadManager.Data.deliverySaveData.clientIds = clinetIds;
        }
    }

    private List<(int, bool)> GetRandomTask()
    {
        int currentLevel = SaveLoadManager.Data.Level;
        var list = deliveryDatabase.Dictionary
            .Where(x => x.Value.level <= currentLevel)
            .OrderBy(x => Guid.NewGuid())
            .Take(maxTaskCount)
            .Select(x => (x.Key, false))
            .ToList();
        return list;
    }

    public void Load(BuildingTaskData buildingTaskData)
    {
    }

    public void OnUIClosed()
    {
        gameManager.PlacementSystem.IsTouchable = true;
    }
}
