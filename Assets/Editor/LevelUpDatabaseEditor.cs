using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LevelUpDatabaseSO))]
public class LevelUpDatabaseEditor : Editor
{
    public override void OnInspectorGUI()
    {
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Save"))
        {
            LevelUpDatabaseSO script = (LevelUpDatabaseSO)target;
            script.Save();
        }
        if (GUILayout.Button("Load"))
        {
            LevelUpDatabaseSO script = (LevelUpDatabaseSO)target;
            EditorUtility.SetDirty(script);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            script.Load();
        }
        EditorGUILayout.EndHorizontal();
        DrawDefaultInspector();
    }
}
