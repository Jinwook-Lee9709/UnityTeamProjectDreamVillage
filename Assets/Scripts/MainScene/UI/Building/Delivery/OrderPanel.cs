using System;
using System.Threading;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OrderPanel : MonoBehaviour
{
    public const string expIconPath = "Sprites/Icons/Icon_Level";
    private static readonly float maskOpacity = 0.9f;
    private static readonly float fadeDuration = 0.3f; 
    private static readonly String portraitPathFormat = "Sprites/Icons/Icon_Portrait_{0}";
    private static readonly String amountFormat = "{0} / {1}";
    private static readonly String compensationItemAmountString = "1";
    private static readonly Color notEnoughColor = Color.red;
    private static readonly Color enoughColor = Color.black;
    
    [SerializeField] private UiManager uiManager;
    
    [SerializeField] private Image portrait;
    [SerializeField] private Image[] itemImages;
    [SerializeField] private Image[] compensationImages;
    [SerializeField] private TextMeshProUGUI[] itemAmountTexts;
    [SerializeField] private TextMeshProUGUI[] compensationAmountTexts;
    [SerializeField] private Image disableMask;
    
    [SerializeField] private Button sendButton;

    public void Init(int deliveryId, DeliveryData data, bool isCleared, Action<int> onButtonClicked, int clientId)
    {
        bool[] isEnough = new bool[3];
        
        gameObject.SetActive(true);
        
        itemImages[0].sprite = GetItemIcon(data.orderItemID1);
        itemImages[1].sprite = GetItemIcon(data.orderItemID2);
        itemImages[2].sprite = GetItemIcon(data.orderItemID3);
        
        portrait.sprite = Resources.Load<Sprite>(String.Format(portraitPathFormat, clientId));

        compensationImages[0].sprite = Resources.Load<Sprite>(PathFormat.goldIconPath);
        compensationAmountTexts[0].text = data.compensationGold.ToString();
        
        compensationImages[1].sprite = Resources.Load<Sprite>(PathFormat.expIconPath);
        compensationAmountTexts[1].text = data.compensationExp.ToString();

        if (data.compensationItem != 0)
        {
            compensationImages[2].sprite = GetItemIcon(data.compensationItem);
            compensationAmountTexts[2].text = compensationItemAmountString;
        }
        else
            compensationImages[2].transform.parent.gameObject.SetActive(false);
        
        isEnough[0] = SetItemText(0, data.orderItemID1, data.requiredCount1);
        isEnough[1] = SetItemText(1, data.orderItemID2, data.requiredCount2);
        isEnough[2] = SetItemText(2, data.orderItemID3, data.requiredCount3);

        DefaultUI defaultUI = uiManager.GetPanel(MainSceneUiIds.Default).GetComponent<DefaultUI>();
        var goldEndPosition = defaultUI.goldImage;
        var expEndPosition = defaultUI.levelImage;
            
        sendButton.interactable = isEnough[0] && isEnough[1] && isEnough[2] && !isCleared;
        sendButton.onClick.RemoveAllListeners();
        sendButton.onClick.AddListener(() =>
            {
                onButtonClicked(deliveryId);
                disableMask.gameObject.SetActive(true);
                disableMask.color = new Color(disableMask.color.r, disableMask.color.g, disableMask.color.b, 0f);
                disableMask.DOFade(maskOpacity, fadeDuration);
                uiManager.iconAnimator.MoveFromUIToUI(compensationImages[0].transform.position, goldEndPosition.position, compensationImages[0].sprite);
                uiManager.iconAnimator.MoveFromUIToUI(compensationImages[1].transform.position, expEndPosition.position, compensationImages[1].sprite);
                if(data.compensationItem != 0)
                    uiManager.iconAnimator.MoveFromUIToUI(compensationImages[2].transform.position, goldEndPosition.position, compensationImages[2].sprite);
            });
        
        disableMask.gameObject.SetActive(isCleared);
    }

    private bool SetItemText(int textNum, int itemId, int amount)
    {
        if (amount == 0)
        {
            itemImages[textNum].transform.parent.gameObject.SetActive(false);
            return true;
        }
        bool isEnough = SaveLoadManager.Data.inventory.IsEnough(itemId, amount);
        int stock = SaveLoadManager.Data.inventory.Get(itemId);
        int displayStock = Math.Clamp(stock, 0, amount);
        
        itemAmountTexts[textNum].color = isEnough ? enoughColor : notEnoughColor;
        itemAmountTexts[textNum].text = string.Format(amountFormat, displayStock, amount);
        
        itemImages[textNum].gameObject.SetActive(true);

        return isEnough;
    }
    

    private Sprite GetItemIcon(int oriderItemId)
    {
        String path = String.Format(PathFormat.itemIconPathWithName, oriderItemId);
        return Resources.Load<Sprite>(path);
    }
    
    
    
}
