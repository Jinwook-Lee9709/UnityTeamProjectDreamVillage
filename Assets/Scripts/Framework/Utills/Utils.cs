using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils
{
    public static int GetAreaNumber(int x, int y)
    {
        return y * Consts.xAxisAreaCount + x + 1;
    }

    public static Vector2 RotatePoint(Vector2 point, Vector2 pivot, float angle)
    {
        float rad = angle * Mathf.Deg2Rad; // 각도를 라디안으로 변환
        float cos = Mathf.Cos(rad);
        float sin = Mathf.Sin(rad);

        // 기준점(pivot)을 기준으로 회전
        Vector2 translated = point - pivot;
        float xNew = translated.x * cos - translated.y * sin;
        float yNew = translated.x * sin + translated.y * cos;

        // 기준점을 다시 더해 원래 위치로 이동
        return new Vector2(xNew, yNew) + pivot;
    }
}
