using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SellPopup : MonoBehaviour
{
    private const int minAmount = 1;

    [SerializeField] private RectTransform touchBoundary;
    [SerializeField] private ItemDatabaseSO itemDatabase;
    [SerializeField] private Button increaseButton;

    [SerializeField] private Button decreaseButton;

    // [SerializeField] private Image itemIcon;
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI itemDescription;
    [SerializeField] private TextMeshProUGUI amountText;
    [SerializeField] private TextMeshProUGUI priceText;
    [SerializeField] private Button sellButton;

    public event UnityAction OnSell;

    private int currentItemId = 0;
    private int currentAmount = 0;

    private int CurrentAmount
    {
        get => currentAmount;
        set
        {
            currentAmount = value;
            HandleButtonActivation();
            UpdateUI();
        }
    }

    private void Awake()
    {
        sellButton.onClick.AddListener(OnSellButtonClicked);
    }

    public void Init(int itemId, UnityAction onSell)
    {
        gameObject.SetActive(true);
        currentItemId = itemId;
        CurrentAmount = minAmount;

        UpdateUI();

        this.OnSell += onSell;
        increaseButton.onClick.AddListener(OnIncreaseAmount);
        decreaseButton.onClick.AddListener(OnDecreaseAmount);
    }

    private void Update()
    {
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);
            bool isSingleTouchEnded = (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled);
            bool isTouchOutOfBound = isSingleTouchEnded &&
                                     !RectTransformUtility.RectangleContainsScreenPoint(touchBoundary, touch.position);
            if (isTouchOutOfBound)
            {
                ClosePopupUI();
            }
        }
    }


    private void OnIncreaseAmount()
    {
        CurrentAmount++;
    }

    private void OnDecreaseAmount()
    {
        CurrentAmount--;
    }

    private void OnSellButtonClicked()
    {
        SaveLoadManager.Data.Gold += itemDatabase.Get(currentItemId).price * CurrentAmount;
        Debug.Log(SaveLoadManager.Data.Gold);
        SaveLoadManager.Data.inventory.RemoveItem(currentItemId, CurrentAmount);
        OnSell.Invoke();
        ClosePopupUI();
    }

    private void HandleButtonActivation()
    {
        int stockAmount = SaveLoadManager.Data.inventory.Get(currentItemId);

        increaseButton.interactable = currentAmount < stockAmount;
        decreaseButton.interactable = currentAmount > minAmount;
    }

    private void UpdateUI()
    {
        ItemData data = itemDatabase.Get(currentItemId);

        amountText.text = CurrentAmount.ToString();

        int totalPrice = data.price * CurrentAmount;
        priceText.text = totalPrice.ToString();
    }

    private void ClosePopupUI()
    {
        increaseButton.onClick.RemoveAllListeners();
        decreaseButton.onClick.RemoveAllListeners();
        gameObject.SetActive(false);
    }
}