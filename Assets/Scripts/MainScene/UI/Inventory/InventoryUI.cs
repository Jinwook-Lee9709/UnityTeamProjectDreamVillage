using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private Transform contents;
    [SerializeField] private ItemSlot itemSlotPrefab;
    [SerializeField] private ItemDatabaseSO itemDatabase;
    [SerializeField] private Button closeButton;

    [SerializeField] private SellPopup sellPopup;

    private void OnEnable()
    {
        SetInventorySlot();

        closeButton.onClick.AddListener(ClosePanel);
    }

    private void SetInventorySlot()
    {
        var inven = SaveLoadManager.Data.inventory.Dictionary;
        foreach (var item in inven)
        {
            var itemData = itemDatabase.Dictionary[item.Key];
            var slot = Instantiate(itemSlotPrefab, contents);
            slot.Init(item.Key, OnSlotClicked);
        }
    }

    public void Refresh()
    {
        foreach (Transform child in contents)
        {
            Destroy(child.gameObject);
        }
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

        gameObject.SetActive(false);
    }
}