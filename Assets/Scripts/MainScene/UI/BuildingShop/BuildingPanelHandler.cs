using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuildingPanelHandler : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI buildingNameText;
    [SerializeField] private TextMeshProUGUI priceText;
    [SerializeField] private Image buildingIcon;
    
    public void Init(int id, BuildingData data)
    {
        buildingNameText.text = DataTableManager.StringTable.Get(String.Format(StringFormat.buildingName, id));
        priceText.text = data.cost.ToString();
    }
}