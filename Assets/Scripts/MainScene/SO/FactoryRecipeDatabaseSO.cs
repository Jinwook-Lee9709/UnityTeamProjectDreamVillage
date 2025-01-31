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
}
