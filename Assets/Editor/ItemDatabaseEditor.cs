using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ItemDatabaseSO))]
public class ItemDatabaseEditor : Editor
{
    public override void OnInspectorGUI()
    {
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Save"))
        {
            ItemDatabaseSO script = (ItemDatabaseSO)target;
            script.Save();
        }
        if (GUILayout.Button("Load"))
        {
            ItemDatabaseSO script = (ItemDatabaseSO)target;
            EditorUtility.SetDirty(script);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            script.Load();
        }
        EditorGUILayout.EndHorizontal();
        DrawDefaultInspector();
    }
}
