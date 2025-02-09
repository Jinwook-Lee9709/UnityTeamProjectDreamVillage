using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FactoryImageHandler : MonoBehaviour
{
    [SerializeField] private Image boxImage;
    [SerializeField] private Image itemImage;
    [SerializeField] private IconAnimator animator;
    [SerializeField] private float animationTime = 0.1f;

    public void SetImage(Sprite boxImage, Sprite itemImage)
    {
        if (this.boxImage.sprite is null)
        {
            this.boxImage.sprite = boxImage;
        }
        else
        {
            if(this.boxImage.sprite != boxImage)
                animator.ChangeSpriteByScaling(this.boxImage, boxImage, animationTime);
        }
            
        
        if (itemImage is not null)
        {
            this.itemImage.enabled = true;
            if (this.itemImage.sprite is null)
            {
                animator.RevealImageByScaling(this.itemImage, itemImage, animationTime);
            }
            else
            {
                if(this.itemImage.sprite != itemImage)
                    animator.RevealImageByScaling(this.itemImage, itemImage, animationTime);
            }
        }
        else
        {
            animator.DisableImageByScaling(this.itemImage, animationTime);
        }
        
    }
}
