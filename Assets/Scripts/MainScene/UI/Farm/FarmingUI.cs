using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class FarmingUI : MonoBehaviour
{
    private static readonly string sicklePath = "Sprites/Icons/Harvest_Icon_Sickle";
    private static readonly string iconPath = "Sprites/Icons/Item_Icon_{0}";
    private static readonly Color activeColor = new Color(1f, 1f, 1f, 1f);
    private static readonly Color inactiveColor = new Color(0.5f, 0.5f, 0.5f, 1f);

    [SerializeField] private Transform poolParent;
    [SerializeField] private GameObject content;
    [SerializeField] private Image prefab;
    [SerializeField] private CropRecipeDatabaseSo cropRecipe;
    [SerializeField] private ItemDatabaseSO itemDatabase;
    [SerializeField] private FarmPopup popup;
    [SerializeField] private RectTransform frameBoundary;
    private ObjectPool<Image> imagePool;
    private List<Image> images = new();
    
    private void Awake()
    {
        imagePool = new ObjectPool<Image>(prefab, poolParent, 20);
    }

    public void ShowPlantUI(Action<int> callback)
    {
        gameObject.SetActive(true);
        int i = 0;
        foreach (var data in cropRecipe.Dictionary)
        {
            if (i == 3)
                break;
            var image = imagePool.GetFromPool();
            image.transform.SetParent(content.transform);
            image.sprite = Resources.Load<Sprite>(string.Format(iconPath, data.Key));
            
            ImageTouchHandler imgTouchHandler = image.gameObject.GetComponent<ImageTouchHandler>();
            imgTouchHandler.OnTouch += (Image image) =>
            {
                callback(data.Key);
                OnItemTouch(data.Key, image);
            };
            bool isAuthorized = SaveLoadManager.Data.gold >= data.Value.necessaryCost;
            imgTouchHandler.interactable = isAuthorized;
            image.material.SetColor("_Color", isAuthorized ? activeColor : inactiveColor);
            
            images.Add(image);
            i++;
        }
    }

    private void OnItemTouch(int itemId, Image image)
    {
        if (Input.touchCount == 1)
        {
            var cropInfo =  cropRecipe.Get(itemId);
            var imageRect = image.rectTransform;
            var frameTopPoint = frameBoundary.GetMiddleTopPosition();
            var popupRect = popup.GetComponent<RectTransform>();
            Vector3 popupPosition = new Vector3(imageRect.position.x, frameTopPoint.y + popupRect.rect.height / 2, 0);
            
            popup.gameObject.SetActive(true);
            popup.SetInfo("name", cropInfo.productionTime, cropInfo.necessaryCost);
            popup.transform.position = popupPosition;
        }
    }

    private void Update()
    {
        if (Input.touchCount > 2)
        {
            popup.gameObject.SetActive(false);
        }
        else if (popup.gameObject.activeSelf && Input.touchCount == 1)
        {
            var touchPos = Input.GetTouch(0).position;
            if (!RectTransformUtility.RectangleContainsScreenPoint(frameBoundary, touchPos))
            {
                popup.gameObject.SetActive(false);
            }
        }
    }
    

    public void ShowHarvestUI(Action callback)
    {
        gameObject.SetActive(true);
        var image = imagePool.GetFromPool();
        image.transform.SetParent(content.transform);
        image.sprite = Resources.Load<Sprite>(sicklePath);
        images.Add(image);
        ImageTouchHandler imgTouchHandler = image.gameObject.GetComponent<ImageTouchHandler>();
        imgTouchHandler.OnTouch += (Image image) => { callback(); };
        imgTouchHandler.interactable = true;

    }


    public void StopFarmingUI()
    {
        foreach (var image in images)
        {
            ImageTouchHandler imgTouchHandler = image.gameObject.GetComponent<ImageTouchHandler>();
            imgTouchHandler.ClearEvent();
            imagePool.ReturnToPool(image);
        }
        images.Clear();
        gameObject.SetActive(false);
    }
}