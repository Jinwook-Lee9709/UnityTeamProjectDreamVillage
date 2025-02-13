using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CropRecipeDatabaseSo))]
public class CropDatabaseEditor : Editor
{
    public override void OnInspectorGUI()
    {
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Save"))
        {
            CropRecipeDatabaseSo script = (CropRecipeDatabaseSo)target;
        }
        if (GUILayout.Button("Load"))
        {
            CropRecipeDatabaseSo script = (CropRecipeDatabaseSo)target;
            script.Load();
            
            EditorUtility.SetDirty(script);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
        EditorGUILayout.EndHorizontal();
        DrawDefaultInspector();
    }
}
