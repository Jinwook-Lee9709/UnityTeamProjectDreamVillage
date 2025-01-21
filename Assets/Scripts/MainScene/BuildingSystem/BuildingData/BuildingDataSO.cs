using System;
using System.Collections;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEngine;

[CreateAssetMenu(fileName = "BuildingDatabase", menuName = "ScriptableObjects/BuildingDatabase", order = 1)]
public class BuildingDatabaseSO : ScriptableObject
{
    [SerializedDictionary, SerializeField] private SerializedDictionary<int, BuildingData> list;
    private void OnEnable()
    {
        list.Clear();
    }

    public void Load()
    {
        list.Clear();
        BuildingDataConverter.Load();
        var table = BuildingDataConverter.GetTable;
        foreach (var pair in table)
        {
            list.Add(pair.Key, pair.Value);
        }
    }
    
    public void Save()
    {
        BuildingDataConverter.Save(list);
    }

    
    
    
    
}

