using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class IconAnimator : MonoBehaviour
{
    private static readonly int PoolSize = 10;
    [Header("Settings")] 
    [SerializeField] private const float moveDuration = 1f;
    [SerializeField] private const float scaleDuration = 1f;
    [SerializeField] private Ease easeType = Ease.OutQuad;
    [SerializeField] private Image prefab;
    [SerializeField] private Transform parent;

    ObjectPool<Image> iconPool;

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
        
        sequence.OnComplete(()=> {iconPool.ReturnToPool(image);});
    }

    public void ChangeSpriteByScaling(Image image, Sprite sprite, float duration = scaleDuration)
    {
        image.transform.DOKill();
        Sequence sequence = DOTween.Sequence();
        sequence.Append(image.transform.DOScale(Vector3.zero, duration).SetEase(easeType));
        sequence.AppendCallback(()=> { image.sprite = sprite; });
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