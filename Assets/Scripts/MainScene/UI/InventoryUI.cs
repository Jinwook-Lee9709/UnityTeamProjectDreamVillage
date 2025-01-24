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
    
    private void OnEnable()
    {
        Dictionary<int, int> dict = SaveLoadManager.Data.inventory;
        foreach (var item in dict)
        {
            var itemData = itemDatabase.Dictionary[item.Key];
            var slot = Instantiate(itemSlotPrefab, contents);
            slot.Init(item.Key);
        }
        closeButton.onClick.AddListener(ClosePanel);
    }

    private void ClosePanel()
    {
        foreach (Transform child in contents)
        {
            Destroy(child.gameObject) ;
        }
        gameManager.PlacementSystem.IsTouchable = true;
        closeButton.onClick.RemoveListener(ClosePanel);
        
        this.gameObject.SetActive(false);
    }
}
