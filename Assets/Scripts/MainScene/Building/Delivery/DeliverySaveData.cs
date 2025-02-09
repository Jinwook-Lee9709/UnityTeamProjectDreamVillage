using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DeliverySaveData
{
    public DateTime lastUpdateTime;
    public List<(int, bool)> deliveryList = new();

}
