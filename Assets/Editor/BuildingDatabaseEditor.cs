using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BuildingDatabaseSO))]
public class BuildingDatabaseEditor : Editor
{
    public override void OnInspectorGUI()
    {
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Save"))
        {
            BuildingDatabaseSO script = (BuildingDatabaseSO)target;
            script.Save();
        }
        if (GUILayout.Button("Load"))
        {
            BuildingDatabaseSO script = (BuildingDatabaseSO)target;
            script.Load();
            
            EditorUtility.SetDirty(script);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
        EditorGUILayout.EndHorizontal();
        DrawDefaultInspector();
    }
}
