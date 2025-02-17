using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class TitleInteractionManager : MonoBehaviour
{
    private static readonly String popupSfxName = "Popup";
    private static readonly String closeSfxName = "Close";

    [SerializeField] private Button optionButton;
    [SerializeField] private Button optionCloseButton;
    [SerializeField] private GameObject panel;
    [SerializeField] private GameObject mainPanel;
    [SerializeField] private Image backgroundImage;
    [SerializeField] private Transform titleImage;

    private AudioClip popupClip;
    private AudioClip closeClip;

    private void Start()
    {
        optionButton.onClick.AddListener(PopupUI);
        optionCloseButton.onClick.AddListener(CloseUI);

        var db = Resources.Load<AudioClipDatabase>(String.Format(PathFormat.soPath, nameof(AudioClipDatabase)));
        popupClip = db.Get(popupSfxName);
        closeClip = db.Get(closeSfxName);

        titleImage.localScale = Vector3.zero;
        titleImage.DOScale(Vector3.one, 0.5f)
            .OnComplete(() =>
            {
                titleImage.DOScale(Vector3.one * 1.1f, 0.8f).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
            }).SetEase(Ease.OutBack).SetLink(titleImage.gameObject);
    }

    private void PopupUI()
    {
        panel.SetActive(true);
        DotAnimator.DissolveInAnimation(backgroundImage);
        DotAnimator.PopupAnimation(mainPanel);
        SoundManager.Instance.PlaySfx(popupClip);
    }

    private void CloseUI()
    {
        DotAnimator.DissolveOutAnimation(backgroundImage);
        DotAnimator.CloseAnimation(mainPanel, onComplete: () => { panel.SetActive(false); });
        SoundManager.Instance.PlaySfx(closeClip);
    }
}