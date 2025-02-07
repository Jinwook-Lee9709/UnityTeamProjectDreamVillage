using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

[Serializable]
public class Inventory
{
    [JsonProperty]
    private Dictionary<int, int> dict = new();
    public Dictionary<int, int> Dictionary { get => dict; }

    public int Get(int index)
    {
        if (dict.ContainsKey(index))
            return dict[index];
        else
            return -1;
    }

    public void AddItem(int item, int amount)
    {
        if (dict.ContainsKey(item))
            dict[item] += amount;
        else
            dict.Add(item, amount);
    }

    public void RemoveItem(int item, int amount)
    {
        if (dict.ContainsKey(item))
        {
            if ((dict[item] -= amount) <= 0)
            {
                dict.Remove(item);
            }
        }
    }

    public bool IsEnough(int item, int amount)
    {
        if (!dict.ContainsKey(item))
            return false;
        
        return dict[item] >= amount;
    }
}