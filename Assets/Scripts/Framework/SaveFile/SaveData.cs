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
    public int level = 1;

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

    public int exp = 0;

    public int Exp
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
    
    public int gold = 500;

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

    public int population = 1;

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

    public Inventory inventory = new();


    public SaveDataV1()
    {
        Version = 1;
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