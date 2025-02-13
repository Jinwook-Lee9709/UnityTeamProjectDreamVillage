using System;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class AreaExpansionUI : MonoBehaviour
{
    private static readonly string itemIconPath = "Sprites/Icons/Item_Icon_{0}";
    private readonly string itemAmountFormat = "{0} / {1}";
    private readonly string coinString = "Coin";
    
    [SerializeField] private Image backGround;
    [SerializeField] private float backgroundAlpha = 0.7f;
    [SerializeField] private GameObject mainPanel;
    [SerializeField] private List<Button> itemButtons;
    [SerializeField] private List<Image> itemImages;
    [SerializeField] private List<TextMeshProUGUI> itemTexts;
    [SerializeField] private Button exitButton;
    [SerializeField] private Button confirmButton;
        
    private int areaId;
    private event Action OnConfirm;
    private Action OnClose;

    private void Awake()
    {
        confirmButton.onClick.AddListener(() => OnConfirm?.Invoke());
        confirmButton.onClick.AddListener(OnExit);
    }

    public void Init(int areaId, AreaRequirementData data, UnityEngine.Events.UnityAction<int> confirmAction, Action onCloseAction = null)
    {
        this.areaId = areaId;

        bool isItem1Enough = UpdatePanel(0, data.needItemId1, data.requiredCount1);
        bool isItem2Enough = UpdatePanel(1, data.needItemId2, data.requiredCount2);
        bool isItem3Enough = UpdatePanel(2, data.needItemId3, data.requiredCount3);

        bool isGoldEnough = UpdateGoldPanel(data.requiredGold);

        confirmButton.interactable = isItem1Enough && isItem2Enough && isItem3Enough && isGoldEnough;

        OnConfirm = null;
        OnConfirm += () => confirmAction(this.areaId);
        
        OnClose = onCloseAction;

        DotAnimator.DissolveInAnimation(backGround, alpha:backgroundAlpha);
        DotAnimator.PopupAnimation(mainPanel);
        
        SoundManager.Instance.PlaySfxByName(AudioNames.Popup.ToString());
    }

    private bool UpdateGoldPanel(int requiredGold)
    {
        string iconPath = string.Format(itemIconPath, coinString);

        int currentGold = Mathf.Clamp(SaveLoadManager.Data.Gold, 0, requiredGold);
        bool isEnough = SaveLoadManager.Data.Gold >= requiredGold;

        UpdatePanelUI(3, iconPath, currentGold, requiredGold, isEnough);

        return isEnough;
    }

    private bool UpdatePanel(int panelNum, int itemId, int requiredAmount)
    {
        string iconPath = string.Format(itemIconPath, itemId);

        int currentStock = Mathf.Clamp(SaveLoadManager.Data.inventory.Get(itemId), 0, requiredAmount);
        bool isEnough = SaveLoadManager.Data.inventory.IsEnough(itemId, requiredAmount);

        UpdatePanelUI(panelNum, iconPath, currentStock, requiredAmount, isEnough);

        return isEnough;
    }

    private void UpdatePanelUI(int panelNum, string iconPath, int currentAmount, int requiredAmount, bool isEnough)
    {
        itemImages[panelNum].sprite = Resources.Load<Sprite>(iconPath);
        itemTexts[panelNum].text = string.Format(itemAmountFormat, currentAmount, requiredAmount);
        itemTexts[panelNum].color = isEnough ? Color.black : Color.red;
    }

    public void OnExit()
    {
        DotAnimator.DissolveOutAnimation(backGround);
        DotAnimator.CloseAnimation(mainPanel, onComplete: () => gameObject.SetActive(false));
        
        SoundManager.Instance.PlaySfxByName(AudioNames.Close.ToString());
        OnClose?.Invoke();
    }
}