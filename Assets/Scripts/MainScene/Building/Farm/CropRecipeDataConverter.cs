using System;
using System.Collections.Generic;

public static class CropRecipeDataConverter
{
    private static readonly string tableName = "CropRecipeTable";
    private static readonly string PATH = "tables/{0}";
    
    static Dictionary<int, CropRecipeData> dict = new ();
    public static Dictionary<int, CropRecipeData> GetTable  { get { return dict; } }
    private class Data
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int Lv { get; set; }
        public int exp { get; set; }
        public float productionTime { get; set; }
        public int productCount  { get; set; }
        public int necessaryCost { get; set; }
    }

    public static void Load()
    {
        dict.Clear();
        string path = String.Format(PATH, tableName);
        var list = DataTable.LoadCsv<Data>(path);
        foreach (var data in list)
        {
            var recipe = new CropRecipeData();
            recipe.name = data.Name;
            recipe.level = data.Lv;
            recipe.exp = data.exp;
            recipe.productionTime = data.productionTime;
            recipe.productCount = data.productCount;
            recipe.necessaryCost = data.necessaryCost;
            dict.Add(data.ID, recipe);
        }
    }
    
}
