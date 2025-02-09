using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactorySmokeAnimation : MonoBehaviour, IBuildingAnimation
{
    [SerializeField] private ParticleSystem[] particle;
    public void OnWorking()
    {
        foreach (var particle in particle)
            particle.Play();
    }

    public void OnIdle()
    {
        foreach (var particle in particle)
            particle.Stop();
    }
}
