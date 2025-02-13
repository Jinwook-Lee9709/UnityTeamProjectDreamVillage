using System;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEngine;
using UnityEngine.UI;

public class TutorialUI : MonoBehaviour
{
    public enum TutorialState
    {
        FirstTutorial,
        DeliveryTutorial,
    }
    [SerializeField] private GameObject panel;
    [SerializeField] private Image backgroundImage;
    [SerializeField] private Button leftButton;
    [SerializeField] private Button rightButton;
    [SerializeField] private PlacementSystem placementSystem;

    [SerializedDictionary, SerializeField] private SerializedDictionary<TutorialState, List<Image>> sequenceDictionary;

    private int currentPage;
    private TutorialState currentTutorialState;

    public void Start()
    {
        leftButton.onClick.AddListener(OnClickLeftButton);
        rightButton.onClick.AddListener(OnClickRightButton);
    }

    public void ShowTutorial(TutorialState state)
    {
        gameObject.SetActive(true);
        DotAnimator.DissolveInAnimation(backgroundImage, alpha: 0.7f);
        DotAnimator.PopupAnimation(panel);
        currentPage = 0;
        sequenceDictionary[state][currentPage].gameObject.SetActive(true);
        currentTutorialState = state;
        CheckIsLastPage();
    }

    public void OnClickLeftButton()
    {
        sequenceDictionary[currentTutorialState][currentPage].gameObject.SetActive(false);
        currentPage--;
        CheckIsLastPage();
        sequenceDictionary[currentTutorialState][currentPage].gameObject.SetActive(true);
    }

    public void OnClickRightButton()
    {
        sequenceDictionary[currentTutorialState][currentPage].gameObject.SetActive(false);
        currentPage++;
        CheckIsLastPage();
        sequenceDictionary[currentTutorialState][currentPage].gameObject.SetActive(true);
    }

    private void CheckIsLastPage()
    {
        bool isLastLeftPage = currentPage == 0;
        leftButton.gameObject.SetActive(!isLastLeftPage);

        bool isLastRightPage = currentPage == sequenceDictionary[currentTutorialState].Count - 1;
        rightButton.gameObject.SetActive(!isLastRightPage);
    }

    public void OnClose()
    {
        sequenceDictionary[currentTutorialState][currentPage].gameObject.SetActive(false);
        placementSystem.IsTouchable = true;
        DotAnimator.DissolveOutAnimation(backgroundImage);
        DotAnimator.CloseAnimation(panel, onComplete: () => gameObject.SetActive(false));
    }
}