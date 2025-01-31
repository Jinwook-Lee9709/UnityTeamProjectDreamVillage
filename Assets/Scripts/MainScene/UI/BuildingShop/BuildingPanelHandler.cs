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
    
    public void Init(BuildingData data)
    {
        buildingNameText.text = data.name;
        priceText.text = data.cost.ToString();
    }
}