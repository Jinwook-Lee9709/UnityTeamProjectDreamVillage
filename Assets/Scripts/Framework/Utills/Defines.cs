using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Languages
{
    Korean,
    English,
}

public enum SoIds
{
    BuildingDatabase,
    CropRecipeDatabase,
    FactoryRecipeDatabase,
    ItemDatabase,
    LevelUpDatabase
}

public enum UniqueBuildingId
{
    Construction = 3000,
    HeliPad = 3015,
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
    World,
    Building,
    Farming,
    Inventory,
    Factory,
    BuildingShop,
    Delivery,
}

[Serializable]
public enum BuildingTypes
{
    Construction = 0,
    Farm,
    Factory,
    Residential,
}

public static class StringFormat
{
    public const string itemName = "ITEM_NAME_{0}";
    public const string itemDesc = "ITEM_DESC_{0}";
    public const string buildingName = "BUILDING_NAME_{0}";
    public const string buildingDesc = "BUILDING_DESC_{0}";
}

public static class PathFormat
{
    public const string soPath = "ScriptableObjects/{0}";
    public const string iconPath = "Sprites/Icons/{0}";
    public const string itemIconPathWithName = "Sprites/Icons/Item_Icon_{0}";
    public const string buildingIconPathWithName = "Sprites/Icons/Building_Icon_{0}";
}

public static class CustomString
{
    public static readonly string nullString = "NULL";
}

public static class Variables
{
    public static Languages currentLanguage = Languages.Korean;
    public static int constructionBuildingId = 3000;
}

public static class StringKeys
{
    public static readonly string notEnough = "NOT_ENOUGH";
    public static readonly string gold = "GOLD";
    public static readonly string exp = "EXP";
    public static readonly string level = "LEVEL";
    public static readonly string population = "POPULATION";
    public static readonly string required = "REQUIRED";
}
public enum ItemType
{
    All,
    Crop,
    Product,
}

public static class Consts
{
    public const int xAxisAreaCount = 6;
    public const int zAxisAreaCount = 6;
    public const int AreaLength = 10;
    public const int StartingArea = 8;
    public const int AreaCount = 32;
}

