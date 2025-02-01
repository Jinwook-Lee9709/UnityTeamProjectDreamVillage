using System.Collections;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEngine;

[CreateAssetMenu(fileName = "FactoryRecipeDatabase", menuName = "ScriptableObjects/FactoryRecipeDatabase")]
public class FactoryRecipeDatabaseSO : ScriptableObject
{
    [SerializedDictionary, SerializeField] private SerializedDictionary<int, FactoryRecipeData> dictionary;
    public SerializedDictionary<int, FactoryRecipeData> Dictionary => dictionary;

    public void Save()
    {
    }

    public void Load()
    {
        dictionary.Clear();
        FactoryRecipeDataConverter.Load();
        var table = FactoryRecipeDataConverter.GetTable;
        foreach (var pair in table)
        {
            dictionary.Add(pair.Key, pair.Value);
        }
    }

    public FactoryRecipeData Get(int key)
    {
        if (dictionary.ContainsKey(key))
            return dictionary[key];
        else
            return null;
    }

    public void CreateMaterial(int id)
    {
        var data = dictionary[id];
        if (IsProductable(id))
        {
            DecreaseMaterial(data.materialID1, data.requiredCount1);
            DecreaseMaterial(data.materialID2, data.requiredCount2);
            DecreaseMaterial(data.materialID3, data.requiredCount3);
        }
    }

    private void DecreaseMaterial(int materialID, int requiredCount)
    {
        if (materialID == 0)
            return;
        SaveLoadManager.Data.inventory.RemoveItem(materialID, requiredCount);
    }

    public bool IsProductable(int id)
    {
        if (!dictionary.ContainsKey(id))
            return false;
        var data = dictionary[id];

        return data.level <= SaveLoadManager.Data.Level &&
               CheckMaterial(data.materialID1, data.requiredCount1) &&
               CheckMaterial(data.materialID2, data.requiredCount2) &&
               CheckMaterial(data.materialID3, data.requiredCount3);
    }

    private bool CheckMaterial(int materialID, int requiredCount)
    {
        if (materialID == 0)
            return true;

        int amount = SaveLoadManager.Data.inventory.Get(materialID);
        return amount >= requiredCount;
    }
}