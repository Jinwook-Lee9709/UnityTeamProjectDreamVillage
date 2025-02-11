using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils
{
    public static int GetAreaNumber(int x, int y)
    {
        return y * Consts.xAxisAreaCount + x + 1;
    }
}
