using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class DefaultUI : MonoBehaviour
{
    [SerializeField] private LevelUpDatabaseSO levelUpDatabase;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI populationText;
    [SerializeField] private TextMeshProUGUI goldText;
    [SerializeField] public Transform populationImage;
    [SerializeField] public Transform levelImage;
    [SerializeField] public Transform goldImage;
    [SerializeField] public Slider expSlider;
    
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
        int needExp = levelUpDatabase.Get(SaveLoadManager.Data.Level).maxExp;
        expSlider.DOValue((float)SaveLoadManager.Data.Exp / needExp, 1f).SetEase(Ease.OutQuad);
    }

    private void OnDestroy()
    {
        SaveLoadManager.OnDataChanged -= OnDataChanged;
    }
}