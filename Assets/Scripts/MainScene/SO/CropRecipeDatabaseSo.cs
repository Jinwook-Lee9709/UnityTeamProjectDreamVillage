using AYellowpaper.SerializedCollections;
using UnityEngine;

[CreateAssetMenu(fileName = "CropRecipeDatabase", menuName = "ScriptableObjects/CropRecipeDatabase")]
public class CropRecipeDatabaseSo : ScriptableObject
{
    [SerializedDictionary, SerializeField] private SerializedDictionary<int, CropRecipeData> dictionary;
    public SerializedDictionary<int, CropRecipeData> Dictionary => dictionary;


    public void Load()
    {
        dictionary.Clear();
        CropRecipeDataConverter.Load();
        var table = CropRecipeDataConverter.GetTable;
        foreach (var pair in table)
        {
            dictionary.Add(pair.Key, pair.Value);
        }
    }

    public CropRecipeData Get(int id)
    {
        if (dictionary.ContainsKey(id))
            return dictionary[id];
        else
            return null;
    }
}