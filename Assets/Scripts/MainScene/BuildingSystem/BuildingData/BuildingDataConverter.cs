using System;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEngine;

public static class BuildingDataConverter
{
    private static readonly string tableName = "BuildingTable";
    private static readonly string PATH = "tables/{0}";
    private static readonly string PrefabPath = "Prefabs/Buildings/Building_Prefab_{0}";
    
    static Dictionary<int, BuildingData> dict = new();
    
    public static Dictionary<int, BuildingData> GetTable  { get { return dict; } }
    private class Data
    {
        public int ID { get; set; }
        public int type { get; set; }
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
            building.buildingType = (BuildingTypes)data.type;
            building.level = data.Lv;
            building.exp = data.exp;
            building.productionTime = data.productionTime;
            building.cost = data.necessaryCost;
            building.prefab = Resources.Load<GameObject>(String.Format(PrefabPath, data.ID));
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
    

}
