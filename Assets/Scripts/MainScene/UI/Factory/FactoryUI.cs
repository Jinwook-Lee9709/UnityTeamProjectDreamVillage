using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class FactoryUI : MonoBehaviour
{
    private static readonly string iconPath = "Sprites/Icons/Item_Icon_{0}";
    private static readonly string timeFormat = "{0:D2} : {1:D2}";

    [SerializeField] private Sprite opendBoxSprite;
    [SerializeField] private Sprite closedBoxSprite;

    //Data
    [SerializeField] private BuildingDatabaseSO buildingDatabase;
    [SerializeField] private FactoryRecipeDatabaseSO recipeDatabase;

    //Transform References
    [SerializeField] private Transform poolParent;
    [SerializeField] private Transform content;

    //Caching Elements
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private Button claimButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private FactoryImageHandler[] boxImages;
    
    //Prefabs
    [SerializeField] private Image bottomSlotPrefab;
    [SerializeField] private Image cursorPrefab;

    //Variables
    private List<Image> images = new();
    private ObjectPool<Image> imagePool;
    private Queue<int> productQueue, completeQueue;
    private int placeId;
    private Coroutine timerCoroutine;
    private Factory factory;

    //For Touch Event
    private bool isTouching;
    private int fingerId = -1;
    private int currentCursorItemId = -1;
    private int availableSlotIndex = -1;
    private Image cursor;

    //Event
    public event Action<int> OnAssignProduct;
    public event Action OnClaimItem;
    public event Action OnQuitFactory;

    private void Awake()
    {
        imagePool = new ObjectPool<Image>(bottomSlotPrefab, poolParent, 20);
        claimButton.onClick.AddListener(OnClaimButtonClicked);
        quitButton.onClick.AddListener(StopUI);
    }

    public void InitUI(int placeId,
        Queue<int> productQueue,
        Queue<int> completeQueue,
        Action<int> onAssignProduct,
        Action onClaim,
        Action onQuit,
        Factory factory)
    {
        this.placeId = placeId;
        this.productQueue = productQueue;
        this.completeQueue = completeQueue;
        this.factory = factory;
        this.OnAssignProduct += onAssignProduct;
        OnClaimItem += onClaim;
        OnQuitFactory += onQuit;

        nameText.text = buildingDatabase.Get(placeId).name;
        
        InitBottmUI();
        UpdateUI();
    }


    private void InitBottmUI()
    {
        var query = recipeDatabase.Dictionary.Where(x => x.Value.placeID == placeId);
        foreach (var data in query)
        {
            var image = imagePool.GetFromPool();
            image.transform.SetParent(content.transform);
            image.sprite = Resources.Load<Sprite>(string.Format(iconPath, data.Key));
            images.Add(image);
            ImageTouchHandler imgTouchHandler = image.gameObject.GetComponent<ImageTouchHandler>();
            imgTouchHandler.OnTouch += (Image image) => OnBottomItemTouched(data.Key);
            imgTouchHandler.interactable = true;
        }
    }

    public void UpdateUI()
    {
        int index = 0;
        foreach (var product in completeQueue)
        {
            SetBoxImage(index, product, closedBoxSprite);
            index++;
            if (index == boxImages.Length)
                return;
        }

        foreach (var product in productQueue)
        {
            SetBoxImage(index, product, opendBoxSprite);
            index++;
            if (index == boxImages.Length)
                return;
        }

        while (index != boxImages.Length)
        {
            boxImages[index].SetImage(opendBoxSprite, null);
            index++;
        }

        if (timerCoroutine == null && productQueue.Count > 0)
        {
            StartCoroutine(TimerUpdateRoutine());
        }
        else if (productQueue.Count == 0)
        {
            if(timerCoroutine != null)
                StopCoroutine(timerCoroutine);
            timerText.text = String.Format(timeFormat, TimeSpan.Zero.Minutes, TimeSpan.Zero.Minutes);
        }
    }

    private void SetBoxImage(int index, int productId, Sprite boxSprite)
    {
        var sprite = Resources.Load<Sprite>(string.Format(iconPath, productId));
        if (sprite == null)
            sprite = Resources.Load<Sprite>(string.Format(iconPath, CustomString.nullString));
        boxImages[index].SetImage(boxSprite, sprite);
    }

    private void OnBottomItemTouched(int id)
    {
        if (Input.touches.Length == 1)
        {
            currentCursorItemId = id;
            fingerId = Input.GetTouch(0).fingerId;
            isTouching = true;
            cursor = Instantiate(cursorPrefab, transform.parent);
            Image img = cursor.GetComponent<Image>();
            cursor.GetComponent<Image>().sprite = Resources.Load<Sprite>(string.Format(iconPath, CustomString.nullString));
            availableSlotIndex = productQueue.Count + completeQueue.Count;
        }
    }

    private void OnClaimButtonClicked()
    {
        OnClaimItem?.Invoke();
    }

    private void Update()
    {
        HandleTouchEvent();
    }

    private void HandleTouchEvent()
    {
        foreach (var touch in Input.touches)
        {
            if (touch.fingerId == fingerId)
            {
                var currentTouch = touch;
                if (currentTouch.phase == TouchPhase.Canceled || currentTouch.phase == TouchPhase.Ended)
                {
                    Destroy(cursor.gameObject);
                    isTouching = false;
                    fingerId = -1;
                    currentCursorItemId = -1;
                }
                if (isTouching)
                {
                    cursor.transform.position = currentTouch.position;
                    if (availableSlotIndex < boxImages.Length && 
                        RectTransformUtility.RectangleContainsScreenPoint(boxImages[availableSlotIndex].GetComponent<Image>().rectTransform,
                            currentTouch.position))
                    {
                        OnAssignProduct.Invoke(currentCursorItemId);
                        availableSlotIndex = productQueue.Count + completeQueue.Count;
                        UpdateUI();
                    }
                }
            }
        }
    }

    private IEnumerator TimerUpdateRoutine()
    {
        while (true)
        {
            if (productQueue != null && productQueue.Count > 0)
            {
                TimeSpan remainTime = factory.remainTime;
                timerText.text = String.Format(timeFormat, remainTime.Minutes, remainTime.Seconds);
            }
            yield return new WaitForSeconds(1f);
        }
    }


    public void StopUI()
    {
        OnQuitFactory?.Invoke();
        
        this.placeId = -1;
        this.productQueue = null;
        this.completeQueue = null;
        this.factory = null;
        OnAssignProduct = null;
        OnClaimItem = null;
        OnQuitFactory = null;
        
        if(timerCoroutine != null)
            StopCoroutine(timerCoroutine);

        foreach (var image in images)
        {
            ImageTouchHandler imgTouchHandler = image.gameObject.GetComponent<ImageTouchHandler>();
            imgTouchHandler.ClearEvent();
            imagePool.ReturnToPool(image);
        }
    }
}