using System;

[Serializable]
public class FarmTaskData : BuildingTaskData
{
    public DateTime? plantedTime;
    public int plantedCropId;
}
