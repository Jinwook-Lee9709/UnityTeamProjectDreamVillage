using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class FeedFactoryAnimation : MonoBehaviour, IBuildingAnimation
{
    [SerializeField] private Transform millFan;
    public void OnWorking()
    {
        millFan.DOKill();
        millFan.localRotation = Quaternion.Euler(0, 0, 0);
        millFan.DOLocalRotate(new Vector3(0, 0, 360), 1f, RotateMode.FastBeyond360).SetEase(Ease.Linear).SetLoops(-1, LoopType.Restart);
    }

    public void OnIdle()
    {
        millFan.DOKill();
        millFan.localRotation = Quaternion.Euler(0, 0, 0); 
    }
}
