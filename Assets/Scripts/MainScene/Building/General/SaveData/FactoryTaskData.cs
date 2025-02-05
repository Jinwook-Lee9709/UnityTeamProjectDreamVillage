using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactoryTaskData : BuildingTaskData
{
    public List<int> productQueue;
    public List<int> completedProductQueue;
    public DateTime productionStartTime;
}
