using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ImageTouchHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private static readonly Color activeColor = new Color(1f, 1f, 1f, 1f);
    private static readonly Color inactiveColor = new Color(0.5f, 0.5f, 0.5f, 1f);
    private Image image;

    private bool interactable = false;
    public bool Interactable
    {
        get => interactable;
        set
        {
            image.color = value ? activeColor : inactiveColor;
            interactable = value;
        }
    }

    public event Action<Image, bool> OnTouch;

    private void Awake()
    {
        image = GetComponent<Image>();
    }
    
    public void ClearEvent()
    {
        OnTouch = null;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnTouch?.Invoke(image, interactable);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
    }
}