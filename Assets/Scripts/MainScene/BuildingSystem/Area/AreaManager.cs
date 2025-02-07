using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class AreaManager : MonoBehaviour
{
    [SerializeField] PlacementSystem placementSystem;
    [SerializeField] List<ParticleSystem> fogParticles;
    [SerializeField] AreaRequirementDatabaseSO areaRequirementDatabase;
    [SerializeField] AreaExpansionUI areaExpansionUI;
    
    private GridData gridData;

    private void Start()
    {
        gridData = placementSystem.GridInfo;
        foreach (var pair in SaveLoadManager.Data.AreaAuthority)
        {
            if (pair.Value)
            {
                fogParticles[pair.Key].gameObject.SetActive(false);
            }
        }
    }
    

    public void UnlockArea(int areaId)
    {
        SaveLoadManager.Data.AreaAuthority[areaId] = true;
        fogParticles[areaId].Stop();

        var data = areaRequirementDatabase.Get(areaId);
        SaveLoadManager.Data.inventory.RemoveItem(data.needItemId1, data.requiredCount1);
        SaveLoadManager.Data.inventory.RemoveItem(data.needItemId2, data.requiredCount2);
        SaveLoadManager.Data.inventory.RemoveItem(data.needItemId3, data.requiredCount3);
        SaveLoadManager.Data.Gold -= data.requiredGold;
    }
    
    private void ShowUnlockRequirementsUI(int areaId)
    {
        areaExpansionUI.gameObject.SetActive(true);
        areaExpansionUI.Init(areaId, areaRequirementDatabase.Get(areaId), UnlockArea);
    }

    public void OnTilePressed(Vector3Int pos)
    {
        int areaId = pos.GetAreaNumber();
        if (SaveLoadManager.Data.AreaAuthority.ContainsKey(areaId))
        {
            ShowUnlockRequirementsUI(areaId);
        }
    }
    
    
}
