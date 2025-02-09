using System;
using UnityEngine;

public class AnimalFactoryAnimation : MonoBehaviour, IBuildingAnimation
{
    static readonly String IdleAnimation = "Idle_A";
    static readonly String WorkingAnimation = "Attack";

    [SerializeField] private Animator[] animator;

    public void OnWorking()
    {
        foreach (Animator animator in animator)
            animator.Play(WorkingAnimation);
        Debug.Log(WorkingAnimation);
    }

    public void OnIdle()
    {
        foreach (Animator animator in animator)
            animator.Play(IdleAnimation);
        Debug.Log(IdleAnimation);
    }
}