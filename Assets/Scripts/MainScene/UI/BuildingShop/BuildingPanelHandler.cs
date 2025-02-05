using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuildingPanelHandler : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI buildingNameText;
    [SerializeField] private GameObject costPlate;
    [SerializeField] private GameObject levelPlate;
    [SerializeField] private GameObject timePlate;
    [SerializeField] private TextMeshProUGUI costText;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI timeText;
    
    [SerializeField] private Image buildingIcon;

    private StringBuilder sb = new();

    public void Init(int id, BuildingData data, bool isAuthorized)
    {
        SetBuildingName(id);
        SetButtonUI(data, isAuthorized);
    }

    private void SetBuildingName(int id)
    {
        buildingNameText.text = DataTableManager.StringTable.Get(String.Format(StringFormat.buildingName, id));
    }

    private void SetButtonUI(BuildingData data, bool isAuthorized)
    {
        sb.Clear();
        ResetPlate();
        if (isAuthorized)
        {
            SetAuthorizedUI(data);
        }
        else
        {
            SetUnauthorizedUI(data);
        }
    }

    private void SetUnauthorizedUI(BuildingData data)
    {
        if (SaveLoadManager.Data.Level <= data.level)
        {
            levelPlate.SetActive(true);
            sb.Append(DataTableManager.StringTable.Get(StringKeys.required));
            sb.Append(DataTableManager.StringTable.Get(StringKeys.level));
            sb.AppendWithBlank(data.level.ToString());
            levelText.text = sb.ToString();
            return;
        }

        if (SaveLoadManager.Data.Gold <= data.cost)
        {
            costPlate.SetActive(true);
            sb.AppendWithBlank(data.cost.ToString());
            costText.text = sb.ToString();
            costText.color = Color.red;
        }
    }

    private void SetAuthorizedUI(BuildingData data)
    {
        costPlate.SetActive(true);
        costText.text = data.cost.ToString();
            
        bool hasProductionTime = !Mathf.Approximately(data.productionTime, 0);
        if (hasProductionTime)
        {
            timePlate.SetActive(true);
            sb.AppendSecondsByTimeString((int)data.productionTime);
            timeText.text = sb.ToString();
        }
    }

    private void ResetPlate()
    {
        costPlate.SetActive(false);
        timePlate.SetActive(false);
        levelPlate.SetActive(false);
        costText.color = Color.black;
        levelText.color = Color.black;
        timeText.color = Color.black;
    }
}