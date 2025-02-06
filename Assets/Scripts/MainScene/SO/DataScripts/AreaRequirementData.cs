using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[Serializable]
public class AreaRequirementData
{
    public int requiredLevel;
    public int needItemId1;
    public int requiredCount1;
    public int needItemId2;
    public int requiredCount2;
    public int needItemId3;
    public int requiredCount3;
    public int requiredGold;
}

public class AreaRequirementDataConverter
{
    private static readonly string tableName = "AreaRequirementTable";
    private static readonly string PATH = "tables/{0}";
    
    static Dictionary<int, AreaRequirementData> dict = new();

    public static Dictionary<int, AreaRequirementData> GetTable => dict;

    private class Data
    {
        public int placeID { get; set; }
        public int levelType { get; set; } 
        public int needItemID1 { get; set; }
        public int itemCount1 { get; set; }
        public int needItemID2 { get; set; }
        public int itemCount2 { get; set; }
        public int needItemID3 { get; set; }
        public int itemCount3 { get; set; }
        public int gold { get; set; }
    }
    
    public static void Load()
    {
        dict.Clear();
        string path = string.Format(PATH, tableName);
        var list = DataTable.LoadCsv<Data>(path);
        foreach (var data in list)
        {
            var requirement = new AreaRequirementData();
            requirement.requiredLevel = data.levelType;
            requirement.needItemId1 = data.needItemID1;
            requirement.needItemId2 = data.needItemID2;
            requirement.needItemId3 = data.needItemID3;
            requirement.requiredCount1 = data.itemCount1;
            requirement.requiredCount2 = data.itemCount2;
            requirement.requiredCount3 = data.itemCount3;
            requirement.requiredGold = data.gold;
            dict.Add(data.placeID, requirement);
        }
    }
}
