using System;
using System.ComponentModel;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static string expString = "Exp";
    [SerializeField] private PlacementSystem placementSystem;
    [SerializeField] private LevelUpDatabaseSO levelUpDatabase;
    public void Start()
    {
        SaveLoadManager.Data.PropertyChanged += OnSaveDataChanged;
    }

    public PlacementSystem PlacementSystem
    {
        get { return placementSystem; }
    }

    public void OnSaveDataChanged(object sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == expString)
        {
            int currentExp = SaveLoadManager.Data.exp;
            int level = SaveLoadManager.Data.level;
            while (true)
            {
                if(levelUpDatabase.Dictionary.ContainsKey(level + 1) == false)
                    break;
                int maxExp = levelUpDatabase.Get(level).maxExp;
                if (currentExp < maxExp)
                    break;
                currentExp -= maxExp;
                level++;
            }
            SaveLoadManager.Data.Exp = currentExp;
            SaveLoadManager.Data.Level = level;
        }
    }
}