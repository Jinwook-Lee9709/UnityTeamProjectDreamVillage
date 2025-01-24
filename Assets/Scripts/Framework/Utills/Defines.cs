using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Languages
{
    Korean,
    English,
}

public static class DataTableIds
{
    public static readonly string Item = "ItemTable";

    public static readonly string[] String =
    {
        "StringTableKr",
        "StringTableEn",
    };
}



[Serializable]
public enum MainSceneUiIds
{
    Default,
    Building,
    Farming,
    Inventory,
}

public static class Variables
{
    public static Languages currentLanguage = Languages.Korean;
}

