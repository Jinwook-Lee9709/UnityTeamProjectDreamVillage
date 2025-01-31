using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FactoryImageHandler : MonoBehaviour
{
    [SerializeField] private Image boxImage;
    [SerializeField] private Image itemImage;

    public void SetImage(Sprite boxImage, Sprite itemImage)
    {
        this.boxImage.sprite = boxImage;
        if (itemImage != null)
        {
            this.itemImage.enabled = true;
            this.itemImage.sprite = itemImage;
        }
        else
        {
            this.itemImage.enabled = false;
        }
        
    }
}
