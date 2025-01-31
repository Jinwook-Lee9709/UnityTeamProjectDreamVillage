using System.Collections;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelUpDatabase", menuName = "ScriptableObjects/LevelUpDatabase")]
public class LevelUpDatabaseSO : ScriptableObject
{
    [SerializedDictionary, SerializeField] private SerializedDictionary<int, LevelUpData> dictionary;
    public SerializedDictionary<int, LevelUpData> Dictionary => dictionary;

    public void Load()
    {
        dictionary.Clear();
        LevelUpDataConverter.Load();
        var table = LevelUpDataConverter.GetTable;
        foreach (var pair in table)
        {
            dictionary.Add(pair.Key, pair.Value);
        }
    }

    public void Save()
    {
        LevelUpDataConverter.Save();
    }

    public LevelUpData Get(int id)
    {
        if (dictionary.ContainsKey(id))
            return dictionary[id];
        else
            return null;
    }
}
