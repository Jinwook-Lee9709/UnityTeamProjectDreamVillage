using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using UnityEngine;

public abstract class SaveData
{
    public int Version { get; protected set; }
    public abstract SaveData VersionUp();
}

public class SaveDataV1 : SaveData, INotifyPropertyChanged
{
    #region Properties

    private int level = 10;

    public int Level
    {
        get => level;
        set
        {
            if (level != value)
            {
                level = value;
                OnPropertyChanged();
            }
        }
    }

    private int exp = 0;

    public int Exp
    {
        get => exp;
        set
        {
            if (exp != value)
            {
                exp = value;
                OnPropertyChanged();
            }
        }
    }

    private int gold = 100000;

    public int Gold
    {
        get => gold;
        set
        {
            if (gold != value)
            {
                gold = value;
                OnPropertyChanged();
            }
        }
    }

    private int population = 1;

    public int Population
    {
        get => population;
        set
        {
            if (population != value)
            {
                population = value;
                OnPropertyChanged();
            }
        }
    }

    #endregion

    public DeliverySaveData deliverySaveData = new();
    
    public Inventory inventory = new();

    public Dictionary<int, bool> AreaAuthority = new();

    public List<BuildingSaveData> buildings = new();
    
    public SaveDataV1()
    {
        Version = 1;
        
        for (int i = 1; i <= Consts.AreaCount; i++)
        {
            AreaAuthority.Add(i, false);
        }

        AreaAuthority[Consts.StartingArea] = true;
    }

    public void OnFirstCreation()
    {
        BuildingSaveData heliPad = new();
        heliPad.buildingId = (int)UniqueBuildingId.HeliPad;
        heliPad.position = new Vector3Int(16, 0 ,18);
        buildings.Add(heliPad);
    }

    public override SaveData VersionUp()
    {
        return new SaveDataV1();
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }
}