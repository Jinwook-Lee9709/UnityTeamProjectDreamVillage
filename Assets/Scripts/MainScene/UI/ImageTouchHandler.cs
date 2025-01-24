using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class ImageTouchHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public event Action onTouch; 
    
    
    public void OnPointerDown(PointerEventData eventData)
    {
        onTouch?.Invoke();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        
    }
}
