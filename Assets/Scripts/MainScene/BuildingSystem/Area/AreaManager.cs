using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaManager : MonoBehaviour
{
    [SerializeField] PlacementSystem placementSystem;
    [SerializeField] List<ParticleSystem> fogParticles;
    [SerializeField] LevelUpDatabaseSO levelUpDatabase;
    
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
            var gridPos = InputManager.Instance.Vector2PositionToPlane(touchedPos).ToVector3Int();
            if (gridData.HasAuthority(gridPos))
            {
            }
            else
            {
                ShowUnlockRequirementsUI(gridPos.GetAreaNumber());
            }
        }
    }

    private void UnLockArea(int areaId)
    {
        SaveLoadManager.Data.AreaAuthority[areaId] = true;
        fogParticles[areaId].Stop();
    }

    private void ShowUnlockRequirementsUI(int areaId)
    {
        
    }
    
    
}
