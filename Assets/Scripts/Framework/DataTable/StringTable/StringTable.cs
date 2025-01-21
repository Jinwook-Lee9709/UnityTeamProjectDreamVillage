using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StringTable : DataTable
{
    private static readonly string errorString = "StringKeyError";
    public class Data
    {
        public string Id { get; set; }
        public string String { get; set; }
    }

    private Dictionary<string, string> dictionary = new();
    
    public override void Load(string filename)
    {
        string path = String.Format(PATH, filename);
        var list = LoadCsv<Data>(path);
        foreach (var data in list)
        {
            if(!dictionary.ContainsKey(data.Id))
                dictionary.Add(data.Id, data.String);
            else
                Debug.LogError($"Key {data.Id} already exists.");
        }
    }

    public string Get(string id)
    {
        if(dictionary.ContainsKey(id))
            return dictionary[id];
        else
            return errorString;
    }
}

