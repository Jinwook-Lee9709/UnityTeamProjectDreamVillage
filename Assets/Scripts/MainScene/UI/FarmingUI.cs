using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FarmingUI : MonoBehaviour
{
    private static readonly string sicklePath = "Sprites/Icons/Harvest_Icon_Sickle";
    private static readonly string iconPath = "Sprites/Icons/Crop_Icon_{0}";
    
    [SerializeField] private Transform poolParent;
    [SerializeField] private GameObject content;
    [SerializeField] private Image prefab;
    [SerializeField] private CropRecipeDatabaseSo cropRecipe;
    [SerializeField] private ItemDatabaseSO itemDatabase;
    private ObjectPool<Image> imagePool;
    private List<Image> images = new List<Image>();

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
            images.Add(image);
            ImageTouchHandler imgTouchHandler = image.gameObject.GetComponent<ImageTouchHandler>();
            imgTouchHandler.onTouch += () => callback(data.Key);
            i++;
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
        imgTouchHandler.onTouch += callback;
    }

    public void StopFarmingUI()
    {
        gameObject.SetActive(false);
        foreach (var image in images)
        {
            imagePool.ReturnToPool(image);
        }
    }
}
