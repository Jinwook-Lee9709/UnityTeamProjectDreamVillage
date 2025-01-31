using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(FactoryRecipeDatabaseSO))]
public class FactoryRecipeDatabaseEditor : Editor
{
    public override void OnInspectorGUI()
    {
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Save"))
        {
            FactoryRecipeDatabaseSO script = (FactoryRecipeDatabaseSO)target;
            script.Save();
        }
        if (GUILayout.Button("Load"))
        {
            FactoryRecipeDatabaseSO script = (FactoryRecipeDatabaseSO)target;
            script.Load();
            
            EditorUtility.SetDirty(script);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
        EditorGUILayout.EndHorizontal();
        DrawDefaultInspector();
    }
}
