using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    private static readonly string IconPath = "Sprites/Icons/Item_Icon_{0}";
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private Button button;
    [SerializeField] private ItemDatabaseSO itemDatabase;

    private int amount;
    private ItemData itemdata;

    public void Init(int itemId, UnityAction<int> onClicked)
    {
        itemdata = itemDatabase.Dictionary[itemId];
        amount = SaveLoadManager.Data.inventory.Get(itemId);
        icon.sprite = Resources.Load<Sprite>(String.Format(IconPath, itemId));
        text.text = amount.ToString();
        button.onClick.AddListener(() => onClicked(itemId));
    }

    private void OnDisable()
    {
        button.onClick.RemoveAllListeners();
    }
}