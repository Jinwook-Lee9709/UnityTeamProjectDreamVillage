using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper;
using UnityEngine;

public abstract class DataTable
{
    public static readonly string PATH = "tables/{0}";

    public abstract void Load(string filename);

    public static List<T> LoadCsv<T>(string path)
    {
        var csv = Resources.Load<TextAsset>(path).text;
        using(var reader = new StringReader(csv))
        using(var csvReader = new CsvReader(reader, CultureInfo.InvariantCulture))
        {
            return csvReader.GetRecords<T>().ToList();
        }
    }
}
