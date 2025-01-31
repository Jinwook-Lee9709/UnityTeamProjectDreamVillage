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
}
