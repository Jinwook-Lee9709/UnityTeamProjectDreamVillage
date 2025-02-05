using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using TMPro;
using Unity.Loading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class FactoryPopup : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI productionTimeText;
    [SerializeField] private Transform resourcesParent;
    [SerializeField] private Transform contents;
    [SerializeField] private FactoryResourceInfoHandler Prefab;
    
    private RectTransform rectTransform;
    private RectTransform contentsRectTransform;

    public void Awake()
    {
        rectTransform = transform as RectTransform;
        contentsRectTransform = contents as RectTransform;
    }
    
    public void SetInfo(int id, FactoryRecipeData data)
    {
        ClearPanel();
        
        String itemName = DataTableManager.StringTable.Get(string.Format(StringFormat.itemName, id));
        nameText.text = itemName;
        productionTimeText.text = data.productionTime.ToString();
        CreateResourceInfo(data.materialID1, data.requiredCount1);
        CreateResourceInfo(data.materialID2, data.requiredCount2);
        CreateResourceInfo(data.materialID3, data.requiredCount3);
    }

    private void CreateResourceInfo(int materialiId, int requiredCount)
    {
        if (materialiId != 0)
        {
            var obj = Instantiate(Prefab, resourcesParent);
            int currentAmount = Mathf.Max(0, SaveLoadManager.Data.inventory.Get(materialiId));
            obj.Init(materialiId, currentAmount, requiredCount);
        }
    }

    public void Update()
    {
        rectTransform.sizeDelta = contentsRectTransform.sizeDelta;
    }

    private void OnDisable()
    {
        ClearPanel();
    }

    private void ClearPanel()
    {
        int childCount = resourcesParent.childCount;
        for (int i = childCount - 1; i >= 0; i--)
        {
            GameObject.Destroy(resourcesParent.GetChild(i).gameObject);
        }
        
    }
}
