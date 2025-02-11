using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleInteractionManager : MonoBehaviour
{
    [SerializeField] private Button optionButton;
    [SerializeField] private Button optionCloseButton;
    [SerializeField] private GameObject panel;
    [SerializeField] private GameObject mainPanel;
    [SerializeField] private Image backgroundImage;
    

    private void Start()
    {
        optionButton.onClick.AddListener(PopupUI);
        optionCloseButton.onClick.AddListener(CloseUI);
    }

    private void PopupUI()
    {
        panel.SetActive(true);
        DotAnimator.DissolveInAnimation(backgroundImage);
        DotAnimator.PopupAnimation(mainPanel);
    }
    
    private void CloseUI()
    {
        DotAnimator.DissolveOutAnimation(backgroundImage);
        DotAnimator.CloseAnimation(mainPanel, onComplete: () => { panel.SetActive(false); });
    }
    
    
}
