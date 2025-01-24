using AYellowpaper.SerializedCollections;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "BuildingDatabase", menuName = "ScriptableObjects/BuildingDatabase")]
public class BuildingDatabaseSO : ScriptableObject
{
    [FormerlySerializedAs("list")] [SerializedDictionary, SerializeField] private SerializedDictionary<int, BuildingData> dictionary;
    public void Load()
    {
        dictionary.Clear();
        BuildingDataConverter.Load();
        var table = BuildingDataConverter.GetTable;
        foreach (var pair in table)
        {
            dictionary.Add(pair.Key, pair.Value);
        }
    }
    
    public void Save()
    {
        BuildingDataConverter.Save(dictionary);
    }

    public BuildingData Get(int id)
    {
        return dictionary[id];
    }
    
    
    
}

