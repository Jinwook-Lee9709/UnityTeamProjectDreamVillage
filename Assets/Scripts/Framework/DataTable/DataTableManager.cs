using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DataTableManager
{
    private static readonly Dictionary<string, DataTable> tables = new ();

    static DataTableManager()
    {
        var table1 = new StringTable();
        table1.Load(Variables.currentLanguage.ToString());
        tables.Add(DataTableIds.String[(int)Variables.currentLanguage], table1);
        
    }
    public static StringTable StringTable
    {
        get
        {
            return Get<StringTable>(DataTableIds.String[(int)Variables.currentLanguage]);
        }
    }
    public static T Get<T>(string id) where T : DataTable
    {
        if (!tables.ContainsKey(id))
        {
            Debug.LogError($"Not found table with id: {id}");
            return null;
        }
        else
        {
            return tables[id] as T;
        }
    }
}
