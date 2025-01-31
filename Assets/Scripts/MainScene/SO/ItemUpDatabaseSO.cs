using AYellowpaper.SerializedCollections;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "ItemDatabase", menuName = "ScriptableObjects/ItemDatabase")]
public class ItemDatabaseSO : ScriptableObject
{
    [SerializedDictionary, SerializeField] private SerializedDictionary<int, ItemData> dictionary;

    public SerializedDictionary<int, ItemData> Dictionary => dictionary;

    public void Load()
    {
        dictionary.Clear();
        ItemDataConverter.Load();
        var table = ItemDataConverter.GetTable;
        foreach (var pair in table)
        {
            dictionary.Add(pair.Key, pair.Value);
        }
    }

    public void Save()
    {
        ItemDataConverter.Save();
    }

    public ItemData Get(int id)
    {
        if (dictionary.ContainsKey(id))
            return dictionary[id];
        else
            return null;
    }
}