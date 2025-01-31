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
    Factory,
    BuildingShop,
}

[Serializable]
public enum BuildingTypes
{
    Farm,
    Factory,
    Decoration,
}

public static class CustomString
{
    public static readonly string nullString = "NULL";
}

public static class Variables
{
    public static Languages currentLanguage = Languages.Korean;
}

