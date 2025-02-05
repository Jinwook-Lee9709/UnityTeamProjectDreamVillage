using System;
using System.ComponentModel;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static string expString = "Exp";
    [SerializeField] private PlacementSystem placementSystem;
    [SerializeField] private LevelUpDatabaseSO levelUpDatabase;
    public void Start()
    {
        SaveLoadManager.Data.PropertyChanged += OnSaveDataChanged;
        
        //for test
        Application.targetFrameRate = 120;
    }

    public PlacementSystem PlacementSystem
    {
        get { return placementSystem; }
    }

    public void OnSaveDataChanged(object sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == expString)
        {
            int currentExp = SaveLoadManager.Data.Exp;
            int level = SaveLoadManager.Data.Level;
            while (true)
            {
                if(levelUpDatabase.Dictionary.ContainsKey(level + 1) == false)
                    break;
                int maxExp = levelUpDatabase.Get(level).maxExp;
                if (currentExp < maxExp)
                    break;
                currentExp -= maxExp;
                level++;
            }
            SaveLoadManager.Data.Exp = currentExp;
            SaveLoadManager.Data.Level = level;
        }
    }
    
    // For test
    [Range(10, 150)]
    public int fontSize = 30;
    public Color color = new Color(.0f, .0f, .0f, 1.0f);
    public float width, height;
    public bool showFrameRate = true;

    public void ToggleFrameRate()
    {
        showFrameRate = !showFrameRate;
    }
    
    void OnGUI()
    {
        if (!showFrameRate)
            return;
        Rect position = new Rect(width, height, Screen.width, Screen.height);

        float fps = 1.0f / Time.deltaTime;
        float ms = Time.deltaTime * 1000.0f;
        string text = string.Format("{0:N1} FPS ({1:N1}ms)", fps, ms);

        GUIStyle style = new GUIStyle();

        style.fontSize = fontSize;
        style.normal.textColor = color;

        GUI.Label(position, text, style);
    }
}