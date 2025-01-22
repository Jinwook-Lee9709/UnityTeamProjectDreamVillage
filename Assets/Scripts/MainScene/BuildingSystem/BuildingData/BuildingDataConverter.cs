using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using AYellowpaper.SerializedCollections;
using CsvHelper;
using UnityEngine;

public static class BuildingDataConverter
{
    private static readonly string tableName = "BuildingTable";
    private static readonly string PATH = "tables/{0}";
    private static readonly string PrefabPath = "Assets/Prefabs/{0}.prefab";
    
    static Dictionary<int, BuildingData> dict = new Dictionary<int, BuildingData>();
    public class Data
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int Lv { get; set; }
        public int exp { get; set; }
        public float productionTime { get; set; }
        public int necessaryCost { get; set; }
    }

    public static void Load()
    {
        dict.Clear();
        string path = String.Format(PATH, tableName);
        var list = DataTable.LoadCsv<Data>(path);
        foreach (var data in list)
        {
            var building = new BuildingData();
            building.name = data.Name;
            building.level = data.Lv;
            building.exp = data.exp;
            building.productionTime = data.productionTime;
            building.cost = data.necessaryCost;
            building.prefab = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>(String.Format(PrefabPath, data.Name));
            building.size = building.prefab?.GetComponent<BuildingSize>()?.size ?? Vector2Int.zero;
            dict.Add(data.ID, building);
        }
    }
    public static void Save(SerializedDictionary<int, BuildingData> dict)
    {
        // List<Data> list = new List<Data>();
        //
        // foreach (var info in dict)
        // {
        //     Data data = new Data();
        //     data.ID = info.Key;
        //     data.Name = info.Value.name;
        //     data.Lv = info.Value.level;
        //     data.exp = info.Value.exp;
        //     data.productionTime = info.Value.productionTime;
        //     data.necessaryCost = info.Value.cost;
        //     list.Add(data);
        // }
        // string path = Path.Combine(Application.dataPath, String.Format($"Resources{PATH}", tableName));
        // using (var writer = new StreamWriter(path))
        // using (var csvWriter = new CsvWriter(writer, CultureInfo.InvariantCulture))
        // {
        //     csvWriter.WriteRecords(list);
        // }
    }
    
    public static Dictionary<int, BuildingData> GetTable  { get { return dict; } }
}
