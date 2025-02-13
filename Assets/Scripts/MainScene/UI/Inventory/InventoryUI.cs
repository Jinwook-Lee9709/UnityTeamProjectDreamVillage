using System;
using System.Collections.Generic;
using System.Linq;
using AYellowpaper.SerializedCollections;
using UnityEngine;
using UnityEngine.Rendering.UI;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [SerializedDictionary, SerializeField] SerializedDictionary<ItemType, Transform> focusPivots = new();
    [SerializeField] private GameManager gameManager;
    [SerializeField] private Transform contents;
    [SerializeField] private ItemSlot itemSlotPrefab;
    [SerializeField] private ItemDatabaseSO itemDatabase;
    [SerializeField] private Button closeButton;
    [SerializeField] private RectTransform tapFocus;
    
    [SerializeField] private SellPopup sellPopup;
    
    private ItemType currentFilterType = ItemType.All;
    
    private void OnEnable()
    {
        currentFilterType = ItemType.All;
        SetInventorySlot();

        closeButton.onClick.AddListener(ClosePanel);
    }

    private void SetInventorySlot()
    {
        foreach (Transform child in contents)
        {
            Destroy(child.gameObject);
        }
        
        var inven = SaveLoadManager.Data.inventory.Dictionary;
        var query = inven
            .Where(x => (ItemType)itemDatabase.Get(x.Key).type == currentFilterType || ItemType.All == currentFilterType )
            .OrderBy(x => x.Key);
        foreach (var item in query)
        {
            var slot = Instantiate(itemSlotPrefab, contents);
            slot.Init(item.Key, OnSlotClicked);
        }
        
        tapFocus.SetParent(focusPivots[currentFilterType]);
        tapFocus.position = focusPivots[currentFilterType].position;
    }

    public void Refresh()
    {
        SetInventorySlot();
    }

    public void OnClickAllCategory()
    {
        currentFilterType = ItemType.All;
        SetInventorySlot();
    }

    public void OnClickCropCategory()
    {
        currentFilterType = ItemType.Crop;
        SetInventorySlot();
    }

    public void OnClickProductCategory()
    {
        currentFilterType = ItemType.Product;
        SetInventorySlot();
    }

    private void OnSlotClicked(int itemId)
    {
        sellPopup.Init(itemId, Refresh);
    }

    private void ClosePanel()
    {
        foreach (Transform child in contents)
        {
            Destroy(child.gameObject);
        }

        gameManager.PlacementSystem.IsTouchable = true;
        closeButton.onClick.RemoveListener(ClosePanel);
        sellPopup.ClosePopupUI();
        
        gameObject.SetActive(false);
    }
}