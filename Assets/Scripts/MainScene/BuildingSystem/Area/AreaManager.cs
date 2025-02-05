using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaManager : MonoBehaviour
{
    [SerializeField] PlacementSystem placementSystem;
    [SerializeField] List<ParticleSystem> fogParticles;
    
    private GridData gridData;

    private void Start()
    {
        gridData = placementSystem.GridInfo;
    }

    private void Update()
    {
        if (MultiTouchManager.Instance.Tap)
        {
            var touchedPos = MultiTouchManager.Instance.lastTapPosition;
            if (gridData.HasAuthority(InputManager.Instance.Vector2PositionToPlane(touchedPos).ToVector3Int()))
            {
            }
            else
            {

            }
        }
    }

}
