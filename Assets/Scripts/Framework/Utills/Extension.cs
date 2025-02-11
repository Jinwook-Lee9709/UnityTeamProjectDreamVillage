using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;

public static class Extension
{
    public static Vector3 GetMiddleTopPosition(this RectTransform rectTransform)
    {
        float topY = rectTransform.position.y +
                     (rectTransform.rect.height * (1 - rectTransform.pivot.y) * rectTransform.lossyScale.y);
        return new Vector3(rectTransform.position.x, topY, rectTransform.position.z);
    }

    public static Vector3Int ToVector3Int(this Vector3 vector3)
    {
        return new Vector3Int(
            Mathf.RoundToInt(vector3.x),
            Mathf.RoundToInt(vector3.y),
            Mathf.RoundToInt(vector3.z)
        );
    }
    public static int GetAreaNumber(this Vector3Int position)
    {
        return position.z / Consts.AreaLength * Consts.xAxisAreaCount + position.x / Consts.AreaLength + 1;
    }

    public static Vector3 ScreenVectorToWorldVector(this Vector2 vector2)
    {
        return new Vector3(vector2.x * 0.5f + vector2.y * 0.5f, 0, -vector2.x * 0.5f + vector2.y * 0.5f);
    }
    


    public static void AppendWithBlank(this StringBuilder sb, string text)
    {
        if (sb.Length > 0)
        {
            sb.Append(" ");
        }
        sb.Append(text);
    }

    public static void AppendSecondsByTimeString(this StringBuilder sb, int seconds)
    {
        sb.Clear();
        int hour = seconds / 3600;
        int minute = seconds % 3600 / 60;
        int second = seconds % 3600 % 60;

        if (hour != 0)
        {
            sb.Append(hour);
            sb.Append(DataTableManager.StringTable.Get("TIME_HOUR"));
        }

        if (minute != 0)
        {
            sb.AppendWithBlank(minute.ToString());
            sb.Append(DataTableManager.StringTable.Get("TIME_MINUTE"));
        }

        if (seconds != 0)
        {
            sb.AppendWithBlank(second.ToString());
            sb.Append(DataTableManager.StringTable.Get("TIME_SECOND"));
        }
    }

    public static bool IsTouchOverUI(this EventSystem eventSystem)
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            PointerEventData eventData = new PointerEventData(EventSystem.current)
            {
                position = touch.position
            };

            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, results);
            return results.Count > 0;
        }
        return false;
    }
}