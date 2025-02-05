using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FactoryResourceInfoHandler : MonoBehaviour
{
    private static readonly Color invalidColor = Color.red;
    private static readonly Color validColor = Color.black;
    
    
    [SerializeField] private Image image;
    [SerializeField] private TextMeshProUGUI text;
    
    public void Init(int itemId, int currentAmount, int needAmount)
    {
        String iconPath = String.Format(PathFormat.iconPathWithName, itemId);
        image.sprite = Resources.Load<Sprite>(iconPath);
        text.text = $"{currentAmount} / {needAmount}";
        text.color = currentAmount > needAmount ? validColor : invalidColor; 
    }
}
