using System;
using System.Collections.Generic;

[Serializable]
public class LevelUpData
{
    public int maxExp;
    public int openPlace;
}

public class LevelUpDataConverter
{
    private static readonly string tableName = "LevelUpTable";
    private static readonly string PATH = "tables/{0}";
    
    static Dictionary<int, LevelUpData> dict = new();
    
    public static Dictionary<int, LevelUpData> GetTable
    {
        get { return dict; }
    }

    private class Data
    {
        public int Lv { get; set; }
        public int maxExp { get; set; }
        public int openPlace { get; set; }
    }

    public static void Load()
    {
        dict.Clear();
        string path = String.Format(PATH, tableName);
        var list = DataTable.LoadCsv<Data>(path);
        foreach (var data in list)
        {
            var levelData = new LevelUpData();
            levelData.maxExp = data.maxExp;
            levelData.openPlace = data.openPlace;
            dict.Add(data.Lv, levelData);
        }
    }

    public static void Save()
    {
        
    }
                                      
    
}
