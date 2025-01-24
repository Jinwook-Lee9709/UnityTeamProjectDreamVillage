using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    private static readonly string IconPath = "Sprites/Icons/Crop_Icon_{0}";
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private ItemDatabaseSO itemDatabase;

    private int amount;
    private ItemData itemdata;
    
    public void Init(int itemId)
    {
        itemdata = itemDatabase.Dictionary[itemId];
        amount = SaveLoadManager.Data.inventory[itemId];
        icon.sprite = Resources.Load<Sprite>(String.Format(IconPath, itemId));
        text.text = amount.ToString();
    }
}
