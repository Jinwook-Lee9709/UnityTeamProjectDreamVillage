using AYellowpaper.SerializedCollections;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "BuildingDatabase", menuName = "ScriptableObjects/BuildingDatabase")]
public class BuildingDatabaseSO : ScriptableObject
{
    [SerializedDictionary, SerializeField] private SerializedDictionary<int, BuildingData> dictionary;
    public SerializedDictionary<int, BuildingData> Dictionary => dictionary;

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
        if(dictionary.ContainsKey(id))
            return dictionary[id];
        else
            return null;
    }
}