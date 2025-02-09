using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class DeliveryData
{
    public int level;
    public int orderItemID1;
    public int requiredCount1;
    public int orderItemID2;
    public int requiredCount2;
    public int orderItemID3;
    public int requiredCount3;
    public int compensationGold;
    public int compensationExp;
    public int compensationItem;
}

public class DeliveryDataConverter
{
    private static readonly string tableName = "DeliveryTable";
    private static readonly string PATH = "tables/{0}";

    static Dictionary<int, DeliveryData> dict = new();

    public static Dictionary<int, DeliveryData> GetTable => dict;

    private class Data
    {
        public int orderID { get; set; }
        public int levelType { get; set; }
        public int orderItemID1 { get; set; }
        public int orderCount1 { get; set; }
        public int orderItemID2 { get; set; }
        public int orderCount2 { get; set; }
        public int orderItemID3 { get; set; }
        public int orderCount3 { get; set; }
        public int compensation_Gold { get; set; }
        public int compensation_XP { get; set; }
        public int compensation_Item { get; set; }
    }

    public static void Load()
    {
        dict.Clear();
        string path = string.Format(PATH, tableName);
        var list = DataTable.LoadCsv<Data>(path);
        foreach (var data in list)
        {
            var info = new DeliveryData();
            info.level = data.levelType;
            info.orderItemID1 = data.orderItemID1;
            info.orderItemID2 = data.orderItemID2;
            info.orderItemID3 = data.orderItemID3;
            info.requiredCount1 = data.orderCount1;
            info.requiredCount2 = data.orderCount2;
            info.requiredCount3 = data.orderCount3;
            info.compensationGold = data.compensation_Gold;
            info.compensationExp = data.compensation_XP;
            info.compensationItem = data.compensation_Item;
            dict.Add(data.orderID, info);
        }
    }
}