using System.Collections;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEngine;

[CreateAssetMenu(fileName = "AreaRequirementDatabase", menuName = "ScriptableObjects/AreaRequirementDatabase")]
public class AreaRequirementDatabaseSO : ScriptableObject
{
 
    [SerializedDictionary, SerializeField] private SerializedDictionary<int, AreaRequirementData> dictionary;
    public SerializedDictionary<int, AreaRequirementData> Dictionary => dictionary;

    public AreaRequirementData Get(int id)
    {
        if(dictionary.ContainsKey(id))
            return dictionary[id];
        return null;
    }
    
    public void Load()
    {
        dictionary.Clear();
        AreaRequirementDataConverter.Load();
        var table = AreaRequirementDataConverter.GetTable;
        foreach (var pair in table)
        {
            dictionary.Add(pair.Key, pair.Value);
        }
    }

    public void Save()
    {
        
    }
}
