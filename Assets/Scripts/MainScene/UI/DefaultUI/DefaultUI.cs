using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class DefaultUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI populationText;
    [SerializeField] private TextMeshProUGUI goldText;

    private void Start()
    {
        levelText.text = SaveLoadManager.Data.Level.ToString();
        populationText.text = SaveLoadManager.Data.Population.ToString();
        goldText.text = SaveLoadManager.Data.Gold.ToString();
        SaveLoadManager.OnDataChanged -= OnDataChanged;
        SaveLoadManager.OnDataChanged += OnDataChanged;
        OnDataChanged();
    }

    private void OnDataChanged()
    {   
        levelText.text = SaveLoadManager.Data.Level.ToString();
        populationText.text = SaveLoadManager.Data.Population.ToString();
        goldText.text = SaveLoadManager.Data.Gold.ToString();
    }

    private void OnDestroy()
    {
        SaveLoadManager.OnDataChanged -= OnDataChanged;
    }
}