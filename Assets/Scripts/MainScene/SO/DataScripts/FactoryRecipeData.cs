using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class FactoryRecipeData
{
    public int level;
    public int exp;
    public float productionTime;
    public int productCount;
    public int materialID1;
    public int requiredCount1;
    public int materialID2;
    public int requiredCount2;
    public int materialID3;
    public int requiredCount3;
    public int placeID;
}

public class FactoryRecipeDataConverter
{
    private static readonly string tableName = "FactoryRecipeTable";
    private static readonly string PATH = "tables/{0}";

    static Dictionary<int, FactoryRecipeData> dict = new();

    public static Dictionary<int, FactoryRecipeData> GetTable => dict;

    private class Data
    {
        public int ID { get; set; }
        public int Lv { get; set; }
        public int exp { get; set; }
        public float productionTime { get; set; }
        public int productCount { get; set; }
        public int itemID_1 { get; set; }
        public int itemCount_1 { get; set; }
        public int itemID_2 { get; set; }
        public int itemCount_2 { get; set; }
        public int itemID_3 { get; set; }
        public int itemCount_3 { get; set; }
        public int placeID { get; set; }
    }

    public static void Load()
    {
        dict.Clear();
        string path = string.Format(PATH, tableName);
        var list = DataTable.LoadCsv<Data>(path);
        foreach (var data in list)
        {
            var recipe = new FactoryRecipeData();
            recipe.level = data.Lv;
            recipe.exp = data.exp;
            recipe.productionTime = data.productionTime;
            recipe.productCount = data.productCount;
            recipe.materialID1 = data.itemID_1;
            recipe.materialID2 = data.itemID_2;
            recipe.materialID3 = data.itemID_3;
            recipe.requiredCount1 = data.itemCount_1;
            recipe.requiredCount2 = data.itemCount_2;
            recipe.requiredCount3 = data.itemCount_3;
            recipe.placeID = data.placeID;
            dict.Add(data.ID, recipe);
        }
    }

    public static void Save()
    {
    }
}