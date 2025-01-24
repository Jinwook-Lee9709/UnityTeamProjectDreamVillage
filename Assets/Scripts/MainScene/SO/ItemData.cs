using System;
using System.Collections.Generic;

[Serializable]
public class ItemData
{
    public int type;
    public int price;
    public string imageID;
}

public class ItemDataConverter
{
    private static readonly string tableName = "ItemTable";
    private static readonly string PATH = "tables/{0}";
    
    static Dictionary<int, ItemData> dict = new ();
    
    public static Dictionary<int, ItemData> GetTable  { get { return dict; } }

    private class Data
    {
        public int ID { get; set; }
        public int type { get; set; }
        public int price { get; set; }
        public string imageID { get; set; }
    }

    public static void Load()
    {
        dict.Clear();
        string path = String.Format(PATH, tableName);
        var list = DataTable.LoadCsv<Data>(path);
        foreach (var data in list)
        {
            var item = new ItemData();
            item.type = data.type;
            item.price = data.price;
            item.imageID = data.imageID;
            dict.Add(data.ID, item);
        }
    }

    public static void Save()
    {
        
    }
}
