using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Factory : MonoBehaviour, IBuilding
{
    [SerializeField] private int placeID;

    //Data
    [SerializeField] private FactoryRecipeDatabaseSO recipeDatabase;

    //References
    private GameManager gameManager;
    private UiManager uiManager;
    private FactoryUI panel;

    //Variable
    private Queue<int> productQueue = new();
    private Queue<int> completedProducts = new();
    private DateTime productionStartTime;
    
    //Properties
    public  TimeSpan remainTime
    {
        get
        {
            float? productionTime = recipeDatabase.Dictionary[productQueue.Peek()]?.productionTime;
            if (productionTime == null)
                return TimeSpan.Zero;
            return productionStartTime.AddSeconds(productionTime.Value) - DateTime.Now;
        }
    }


    public void OnTouch()
    {
        panel.gameObject.SetActive(true);
        panel.InitUI(placeID,productQueue, completedProducts, 
            OnAssignProduct, OnClaim, OnQuit,this);
        gameManager.PlacementSystem.IsTouchable = false;
    }

    public void Init(GameManager gameManager, UiManager uiManager, bool isFirst)
    {
        this.gameManager = gameManager;
        this.uiManager = uiManager;
        panel = uiManager.GetPanel(MainSceneUiIds.Factory).GetComponent<FactoryUI>();
    }

    public void Update()
    {
        CheckProductionCompleted();
    }

    private void CheckProductionCompleted()
    {
        if (productQueue.Count == 0)
            return;
        float productionTime = recipeDatabase.Dictionary[productQueue.Peek()].productionTime;
        if (productionStartTime.AddSeconds(productionTime) < DateTime.Now)
        {
            int completedProductId = productQueue.Dequeue();
            completedProducts.Enqueue(completedProductId);
            productionStartTime = DateTime.Now;
            if (panel.gameObject.activeSelf)
            {
                panel.UpdateUI();
            }
        }
    }

    private void OnAssignProduct(int id)
    {
        if (recipeDatabase.IsProductable(id))
        {
            var data = recipeDatabase.Get(id);
            productQueue.Enqueue(id);
            if (productQueue.Count == 1)
                productionStartTime = DateTime.Now;
            recipeDatabase.CreateMaterial(id);
        }
    }

    private void OnClaim()
    {
        var inventory = SaveLoadManager.Data.inventory;
        foreach (int item in completedProducts)
        {
            var itemData = recipeDatabase.Get(item);
            int productAmount = itemData.productCount;
            int earnExp = itemData.exp;
            
            SaveLoadManager.Data.inventory.AddItem(item, productAmount);
            SaveLoadManager.Data.Exp += earnExp;
        }
        completedProducts.Clear();
        panel.UpdateUI();
    }

    private void OnQuit()
    {
        panel.gameObject.SetActive(false);
        gameManager.PlacementSystem.IsTouchable = true;
    }
}