using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class TextAnimator : MonoBehaviour
{
    [SerializeField] private int charactersPerSecond = 15;
    
    private TextMeshProUGUI text;

    private void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        Debug.Log("Flag!!");
        String textBuf = text.text;
        text.text = String.Empty;
        text.DOText(textBuf, (float) textBuf.Length / charactersPerSecond)
            .SetEase(Ease.Linear)
            .OnKill(() => { text.text = textBuf; });
        
    } 
    
}
