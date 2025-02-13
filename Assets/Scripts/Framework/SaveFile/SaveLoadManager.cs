using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;
using Directory = System.IO.Directory;
using SaveDataVC = SaveDataV1;

public class SaveLoadManager
{
    private static string SaveDirectory = $"{Application.persistentDataPath}/Save";
    public static int SaveDataVersion { get; private set; } = 1;

    private static SaveDataVC data;

    public static event Action OnBeforeSave;

    public static SaveDataVC Data
    {
        get => data;
        set
        {
            if (data != value)
            { 
                if (data != null)
                {
                    data.PropertyChanged -= HandlePropertyChanged;
                    OnDataChanged?.Invoke();
                }
                data = value;
                if (data != null)
                {
                    data.PropertyChanged += HandlePropertyChanged;
                }
                OnDataChanged?.Invoke();
            }

           
        }
    }

    public static readonly string[] SaveFileName =
    {
        "AutoSave",
        "SaveFile1",
        "SaveFile2",
        "SaveFile3"
    };

    public static event Action OnDataChanged;
    
    private static void HandlePropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        OnDataChanged?.Invoke();
    }

    private static JsonSerializerSettings settings = new JsonSerializerSettings()
    {
        Formatting = Formatting.Indented,
        TypeNameHandling = TypeNameHandling.All,
    };

    public static bool IsSaveExists()
    {
        return Directory.Exists(SaveDirectory);
    }
    

    public static bool Save(int slot = 0)
    {
        if (Data == null || slot < 0 || slot >= SaveFileName.Length)
            return false;
        if (!Directory.Exists(SaveDirectory))
        {
            Directory.CreateDirectory(SaveDirectory);
        }
        OnBeforeSave?.Invoke();
        var path = Path.Combine(SaveDirectory, SaveFileName[slot]);
        var json = JsonConvert.SerializeObject(Data, settings);
        File.WriteAllText(path, json);
        return true;
    }

    public static void DeleteSave()
    {
        DirectoryInfo dir = new DirectoryInfo (SaveDirectory);
        dir.Attributes=System.IO.FileAttributes.Normal;
        dir.Delete(true);
    }

    public static async UniTask Load(int slot = 0)
    {
        if (slot < 0 || slot >= SaveFileName.Length)
            return ;

        var path = Path.Combine(SaveDirectory, SaveFileName[slot]);
        if (!Directory.Exists(SaveDirectory))
        {
            Data = new SaveDataVC();
            Data.OnFirstCreation();
            Save();
            
        }

        string json;
        try
        {
            json = await File.ReadAllTextAsync(path); // 비동기 파일 읽기
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error reading file: {ex.Message}");
            return;
        }
        var saveData = JsonConvert.DeserializeObject<SaveData>(json, settings);
        while (saveData.Version < SaveDataVersion)
        {
            saveData = saveData.VersionUp();
        }

        Data = saveData as SaveDataVC;
    }
}