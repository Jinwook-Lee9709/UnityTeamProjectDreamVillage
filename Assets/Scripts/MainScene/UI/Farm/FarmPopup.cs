using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FarmPopup : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI productionTimeText;
    [SerializeField] private TextMeshProUGUI costText;

    public void SetInfo(string name, float productionTime, int cost)
    {
        nameText.text = name;
        productionTimeText.text = productionTime.ToString();
        costText.text = cost.ToString();
    }
}
