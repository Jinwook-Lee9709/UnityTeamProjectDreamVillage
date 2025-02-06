using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AreaRequirementDatabaseSO))]
public class AreaRequirementEditor : Editor
{
    public override void OnInspectorGUI()
    {
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Save"))
        {
            AreaRequirementDatabaseSO script = (AreaRequirementDatabaseSO)target;
            script.Save();
        }
        if (GUILayout.Button("Load"))
        {
            AreaRequirementDatabaseSO script = (AreaRequirementDatabaseSO)target;
            script.Load();
            
            EditorUtility.SetDirty(script);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
        EditorGUILayout.EndHorizontal();
        DrawDefaultInspector();
    }
}
