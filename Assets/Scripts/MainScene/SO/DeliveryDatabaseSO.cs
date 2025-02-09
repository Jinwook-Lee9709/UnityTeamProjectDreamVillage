using System.Collections;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEngine;

[CreateAssetMenu(fileName = "DeliveryDatabase", menuName = "ScriptableObjects/DeliveryDatabase")]
public class DeliveryDatabaseSO : ScriptableObject
{
    [SerializedDictionary, SerializeField] private SerializedDictionary<int, DeliveryData> dictionary;
    public SerializedDictionary<int, DeliveryData> Dictionary => dictionary;
        
    public void Save()
    {
    }

    public void Load()
    {
        dictionary.Clear();
        DeliveryDataConverter.Load();
        var table = DeliveryDataConverter.GetTable;
        foreach (var pair in table)
        {
            dictionary.Add(pair.Key, pair.Value);
        }
    }
    
    public DeliveryData Get(int key)
    {
        if (dictionary.ContainsKey(key))
            return dictionary[key];
        else
            return null;
    }
}
