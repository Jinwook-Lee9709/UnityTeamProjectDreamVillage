using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public static class DotAnimator
{
    private const float animationDuration = 0.3f;

    public static void PopupAnimation(GameObject obj, float duration = animationDuration, Action onComplete = null)
    {
        obj.transform.DOKill();
        obj.SetActive(true);
        obj.transform.localScale = Vector3.zero;
        obj.transform.DOScale(Vector3.one, duration).OnComplete(() => onComplete?.Invoke());
    }

    public static void CloseAnimation(GameObject obj, float duration = animationDuration, Action onComplete = null)
    {
        obj.transform.DOKill();
        obj.transform.DOScale(Vector3.zero, duration).OnComplete(() => onComplete?.Invoke());
    }

    public static void DissolveInAnimation(Image image, float duration = animationDuration, float alpha = 1.0f,
        Action onComplete = null)
    {
        image.DOKill();
        image.color = new Color(image.color.r, image.color.g, image.color.b, 0f);
        image.DOFade(alpha, duration).OnComplete(() => onComplete?.Invoke());
        ;
    }

    public static void DissolveOutAnimation(Image image, float duration = animationDuration, Action onComplete = null)
    {
        image.DOKill();
        image.DOFade(0, duration).OnComplete(() => onComplete?.Invoke());
    }
}