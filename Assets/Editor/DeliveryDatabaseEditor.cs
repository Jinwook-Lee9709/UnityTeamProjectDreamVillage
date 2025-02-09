using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DeliveryDatabaseSO))]
public class DeliveryDatabaseEditor : Editor
{
    public override void OnInspectorGUI()
    {
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Save"))
        {
            DeliveryDatabaseSO script = (DeliveryDatabaseSO)target;
            script.Save();
        }
        if (GUILayout.Button("Load"))
        {
            DeliveryDatabaseSO script = (DeliveryDatabaseSO)target;
            script.Load();
            
            EditorUtility.SetDirty(script);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
        EditorGUILayout.EndHorizontal();
        DrawDefaultInspector();
    }
}
