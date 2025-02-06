using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BuildingSaveDataManager
{
   public static BuildingTaskData MakeTaskData(IBuilding building)
   {
      BuildingTaskData taskData = null;
      switch (building)
      {
         case Farm farm:
         {
            FarmTaskData farmTaskData = new();
            farmTaskData.plantedTime = farm.PlantedTime;
            farmTaskData.plantedCropId = farm.PlantedCropId;
            taskData = farmTaskData;
            break;
         }
         case Factory factory:
         {
            FactoryTaskData factoryTaskData = new();
            factoryTaskData.completedProductQueue = factory.ProductQueue.ToList();
            factoryTaskData.productQueue = factory.CompletedProducts.ToList();
            factoryTaskData.productionStartTime = factory.ProductionStartTime;
            taskData = factoryTaskData;
            break;
         }
         case Construction construction:
         {
            ConstructionTaskData constructionTaskData = new();
            constructionTaskData.startTime = construction.StartTime;
            constructionTaskData.buildingDataId = construction.BuildingDataId;
            taskData = constructionTaskData;
            break;
         }
         case Residential:
         {
            break;
         }
      }
      return taskData;
   }
}
