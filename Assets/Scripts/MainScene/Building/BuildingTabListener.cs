using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingTabListener : MonoBehaviour
{
    [SerializeField] private Grid grid;
    [SerializeField] private ObjectPlacer objectPlacer;
    [SerializeField] private PlacementSystem placementSystem;
    
    private void Update()
    {
        if (MultiTouchManager.Instance.Tap && placementSystem.IsTouchable)
        {
            Vector3 planePos = InputManager.Instance.Vector2PositionToPlane(MultiTouchManager.Instance.lastTapPosition);
            Vector3Int tilePos = grid.WorldToCell(planePos);
            int guid = placementSystem.GridInfo.GetGuid(tilePos);
            if (guid != -1)
            {
                var obj = objectPlacer.ObjectDictionary[guid];
                obj.GetComponentInChildren<IBuilding>()?.OnTouch();
            }
        }
    }
}
