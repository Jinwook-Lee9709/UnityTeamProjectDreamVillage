using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}
