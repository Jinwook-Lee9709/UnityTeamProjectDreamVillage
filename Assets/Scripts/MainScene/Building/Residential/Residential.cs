using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Residential : MonoBehaviour, IBuilding
{
    [SerializeField] private int placeID;
    [SerializeField] private BuildingDatabaseSO buildingDatabaseSo;
    private GameManager gameManager;
    private UiManager uiManager;
    
    public void OnTouch()
    {
        
    }

    public void Init(GameManager gameManager, UiManager uiManager, bool IsFirst = true)
    {
       this.gameManager = gameManager;
       this.uiManager = uiManager;
       if (IsFirst)
       {
       }
    }
}
