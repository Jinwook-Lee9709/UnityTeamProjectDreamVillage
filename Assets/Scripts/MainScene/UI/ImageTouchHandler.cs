using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ImageTouchHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private Image image;
    public bool interactable;
    public event Action<Image> OnTouch;

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
        if(interactable)
            OnTouch?.Invoke(image);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
    }
}