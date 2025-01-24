using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

[CustomEditor(typeof(ScrollRect))]
public class ScrollRectEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        // ScrollRect scrollRect = (ScrollRect)target;
        //
        // // 슬라이더 UI 추가
        // GUILayout.Label("Horizontal Position");
        // float newPosition = GUILayout.HorizontalSlider(scrollRect.horizontalNormalizedPosition, 0f, 1f);
        //
        // // 값 변경 시 업데이트
        // if (Mathf.Abs(newPosition - scrollRect.horizontalNormalizedPosition) > 0.001f)
        // {
        //     Undo.RecordObject(scrollRect, "Change Scroll Position");
        //     scrollRect.horizontalNormalizedPosition = newPosition;
        // }
        
        
    }
}
