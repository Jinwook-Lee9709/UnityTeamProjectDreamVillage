using System;
using System.IO.Pipes;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class DeliveryUI : MonoBehaviour
{
    [SerializeField] PlacementSystem placementSystem;
    [SerializeField] DeliveryDatabaseSO deliveryDatabase;
    [SerializeField] private OrderPanel[] orderPanels;
    [SerializeField] private GameObject mainPanel;
    [SerializeField] private Image backgroundImage;

    private event Action OnClosed;
    public void Init(Action action)
    {
        OnClosed = null;
        OnClosed += action;
        UpdateOrderPanels();
        
        DotAnimator.DissolveInAnimation(backgroundImage, alpha:0.7f);
        DotAnimator.PopupAnimation(mainPanel);
        SoundManager.Instance.PlaySfxByName(AudioNames.Popup.ToString());
    }

    private void UpdateOrderPanels()
    {
        for (int i = 0; i < orderPanels.Length; i++)
        {
            var pair = 
                SaveLoadManager.Data.deliverySaveData.deliveryList[i];
            int portraitIndex = SaveLoadManager.Data.deliverySaveData.clientIds[i];
            DeliveryData deliveryData = deliveryDatabase.Get(pair.Item1);
            orderPanels[i].Init(pair.Item1, deliveryData, pair.Item2, OnSend, portraitIndex);
        }
    }

    public void OnSend(int taskId)
    {
        var task = deliveryDatabase.Get(taskId);
        SaveLoadManager.Data.inventory.RemoveItem(task.orderItemID1,task.requiredCount1);
        if(task.compensationItem != 0)
            SaveLoadManager.Data.inventory.AddItem(task.compensationItem, 1);
        SaveLoadManager.Data.Gold += task.compensationGold;
        SaveLoadManager.Data.Exp += task.compensationExp;
        var index = SaveLoadManager.Data.deliverySaveData.deliveryList.FindIndex(x => x.Item1 == taskId);
        SaveLoadManager.Data.deliverySaveData.deliveryList[index] = (taskId, true);

            
        UpdateOrderPanels();
    }

    public void OnClose()
    {
        placementSystem.IsTouchable = false;
        DotAnimator.DissolveOutAnimation(backgroundImage, onComplete:() =>
        {
            gameObject.SetActive(false);
            placementSystem.IsTouchable = true;
        });
        DotAnimator.CloseAnimation(mainPanel);
        SoundManager.Instance.PlaySfxByName(AudioNames.Close.ToString());
        
        OnClosed?.Invoke();
    }
    
}
