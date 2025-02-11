using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class IconAnimator : MonoBehaviour
{
    private static readonly int PoolSize = 10;
    [Header("Settings")] 
    private const float moveDuration = 0.3f;
    private const float scaleDuration = 0.3f;
    [SerializeField] private Ease easeType = Ease.OutQuad;
    [SerializeField] private Image prefab;
    [SerializeField] private Transform parent;
    [SerializeField] private Transform underIconParent;
    [SerializeField] private Transform worldUiCanvas;

    private ObjectPool<Image> iconPool;
    // private List<Image> pool;

    private void Awake()
    {
        iconPool = new ObjectPool<Image>(prefab, parent, PoolSize);
    }

    public void MoveFromWorldToUI(Vector3 startWorldPos, Vector2 endPos, Sprite sprite, float duration = moveDuration)
    {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(startWorldPos);
        var image = iconPool.GetFromPool();
        image.transform.position = screenPos;
        image.transform.localScale = Vector3.zero;
        image.sprite = sprite;

        Sequence sequence = DOTween.Sequence();

        sequence.Append(image.transform.DOScale(Vector3.one, duration).SetEase(easeType));
        sequence.Append(image.transform.DOMove(endPos, duration).SetEase(easeType));
        sequence.Append(image.transform.DOScale(Vector3.zero, duration).SetEase(easeType));

        sequence.OnComplete(() => { iconPool.ReturnToPool(image); });
    }

    public Image PopupIconOnBuildingPos(Sprite sprite, Vector3 worldPos)
    {
        var image = iconPool.GetFromPool();
        image.transform.SetParent(worldUiCanvas);
        image.transform.position = worldPos;
        image.rectTransform.sizeDelta = Vector2.one;
        image.transform.rotation = Camera.main.transform.rotation;
        RevealImageByScaling(image, sprite);
        return image;
    }

    public void DisablePopupIcon(Image image)
    {
        image.transform.DOScale(Vector3.zero, scaleDuration).OnComplete(() => { iconPool.ReturnToPool(image); });
    }

    public void MoveFromUIToUI(Vector3 startPos, Vector2 endPos, Sprite sprite, float duration = moveDuration)
    {
        var image = iconPool.GetFromPool();
        image.transform.position = startPos;
        image.transform.localScale = Vector3.zero;
        image.sprite = sprite;

        Sequence sequence = DOTween.Sequence();

        sequence.Append(image.transform.DOScale(Vector3.one, duration).SetEase(easeType));
        sequence.Append(image.transform.DOMove(endPos, duration).SetEase(easeType));
        sequence.Append(image.transform.DOScale(Vector3.zero, duration).SetEase(easeType));

        sequence.OnComplete(() => { iconPool.ReturnToPool(image); });
    }

    public void ChangeSpriteByScaling(Image image, Sprite sprite, float duration = scaleDuration)
    {
        image.transform.DOKill();
        Sequence sequence = DOTween.Sequence();
        sequence.Append(image.transform.DOScale(Vector3.zero, duration).SetEase(easeType));
        sequence.AppendCallback(() => { image.sprite = sprite; });
        sequence.Append(image.transform.DOScale(Vector3.one, duration).SetEase(easeType));
    }

    public void RevealImageByScaling(Image image, Sprite sprite, float duration = scaleDuration)
    {
        image.transform.DOKill();
        image.transform.localScale = Vector3.zero;
        image.sprite = sprite;
        image.transform.DOScale(Vector3.one, duration).SetEase(easeType);
    }

    public void DisableImageByScaling(Image image, float duration = scaleDuration)
    {
        image.transform.DOKill();
        image.transform.DOScale(Vector3.zero, duration).SetEase(easeType)
            .OnComplete(() =>
            {
                image.enabled = false;
                image.sprite = null;
            });
    }
}