using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;
using Directory = System.IO.Directory;
using SaveDataVC = SaveDataV1;

public class SaveLoadManager : MonoBehaviour
{
    private static string SaveDirectory = $"{Application.persistentDataPath}/Save";
    
    public static int SaveDataVersion { get; private set; } = 1;
    
    public static SaveDataVC Data { get; set; }
    
    public static readonly string[] SaveFileName =
    {
        "AutoSave",
        "SaveFile1",
        "SaveFile2",
        "SaveFile3"
    };

    private static JsonSerializerSettings settings = new JsonSerializerSettings()
    {
        Formatting = Formatting.Indented,
        TypeNameHandling = TypeNameHandling.All,
    };

    static SaveLoadManager()
    {
        if (!Load())
        { 
            Data = new SaveDataVC();
            Save();
        }
    }
    
    public static bool Save(int slot = 0)
    {
        if (Data == null || slot < 0 || slot >= SaveFileName.Length)
            return false;
        if(!Directory.Exists(SaveDirectory))
        {
            Directory.CreateDirectory(SaveDirectory);
        }
        
        var path = Path.Combine(SaveDirectory, SaveFileName[slot]);
        var json = JsonConvert.SerializeObject(Data, settings);
        File.WriteAllText(path, json);
        return true;
    }
    
    public static bool Load(int slot = 0)
    {
        if (slot < 0 || slot >= SaveFileName.Length)
            return false;
        
        var path = Path.Combine(SaveDirectory, SaveFileName[slot]);
        if (!Directory.Exists(SaveDirectory))
            return false;
        
        var json = File.ReadAllText(path);
        var saveData = JsonConvert.DeserializeObject<SaveData>(json, settings);
        while (saveData.Version < SaveDataVersion)
        {
            saveData = saveData.VersionUp();
        }
        Data = saveData as SaveDataVC;
        ;       return true;
    }


}
