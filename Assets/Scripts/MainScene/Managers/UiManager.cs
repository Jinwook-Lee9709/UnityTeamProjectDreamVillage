using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    [SerializedDictionary, SerializeField] private SerializedDictionary<MainSceneUiIds, GameObject> UiObjects;
    [SerializeField] private Button buildingShopButton;
    [SerializeField] public IconAnimator iconAnimator;
    
    public GameObject GetPanel(MainSceneUiIds id)
    {
        if (UiObjects.ContainsKey(id))
        {
            return UiObjects[id];
        }
        return null;
        
    }

    public void SetDefaultUiInteract(bool isInteractable)
    {
        buildingShopButton.interactable = isInteractable;
    }
    
}