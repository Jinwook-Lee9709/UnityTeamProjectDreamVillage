using System;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OrderPanel : MonoBehaviour
{
    private static readonly String amountFormat = "{0} / {1}";
    private static readonly Color notEnoughColor = Color.red;
    private static readonly Color enoughColor = Color.black;
    
    [SerializeField] private Image[] itemImages;
    [SerializeField] private Image portrait;
    [SerializeField] private TextMeshProUGUI[] itemAmountTexts;
    [SerializeField] private Button sendButton;

    public void Init(int deliveryId, DeliveryData data, bool isCleared,Action<int> onButtonClicked)
    {
        bool[] isEnough = new bool[3];
        
        gameObject.SetActive(true);
        
        itemImages[0].sprite = GetItemIcon(data.orderItemID1);
        itemImages[1].sprite = GetItemIcon(data.orderItemID2);
        itemImages[2].sprite = GetItemIcon(data.orderItemID3);

        bool flag1 = SetText(0, data.orderItemID1, data.requiredCount1);
        bool flag2 = SetText(1, data.orderItemID2, data.requiredCount2);
        bool flag3 = SetText(2, data.orderItemID3, data.requiredCount3);

        sendButton.interactable = flag1 && flag2 && flag3 && !isCleared;
        
        sendButton.onClick.AddListener(() => onButtonClicked(deliveryId));
    }

    private bool SetText(int textNum, int itemId, int amount)
    {
        if (amount == 0)
        {
            itemImages[textNum].gameObject.SetActive(false);
            return false;
        }
        bool isEnough = SaveLoadManager.Data.inventory.IsEnough(itemId, amount);
        int stock = SaveLoadManager.Data.inventory.Get(itemId);
        int displayStock = Math.Clamp(stock, 0, amount);
        
        itemAmountTexts[textNum].color = isEnough ? enoughColor : notEnoughColor;
        itemAmountTexts[textNum].text = string.Format(amountFormat, displayStock, amount);

        return isEnough;
    }

    private Sprite GetItemIcon(int oriderItemId)
    {
        String path = String.Format(PathFormat.itemIconPathWithName, oriderItemId);
        Debug.Log(path);
        return Resources.Load<Sprite>(path);
    }
    
    
    
}
