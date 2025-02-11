using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class AreaManager : MonoBehaviour
{
    private static readonly string lockIconPath = "Sprites/Icons/Icon_Lock";
    private static readonly Vector3 CameraOffset = new Vector3(-13f, 15f, -13f);
    private static readonly float iconYOffset = 4f;

    [Header("SettingValue")] [SerializeField]
    private float moveDuration = 1f;

    [SerializeField] private Ease cameraMoveEase;

    [Header("References")] [SerializeField]
    private UiManager uiManager;

    [SerializeField] private PlacementSystem placementSystem;
    [SerializeField] private List<ParticleSystem> fogParticles;
    [SerializeField] private List<Transform> groundList;
    [SerializeField] private MeshRenderer indicatorImage;
    [SerializeField] private AreaRequirementDatabaseSO areaRequirementDatabase;
    [SerializeField] private AreaExpansionUI areaExpansionUI;
    [SerializeField] private Material originalFloorMaterial;

    private Dictionary<int, Image> lockIcons = new();
    private GridData gridData;

    private void Start()
    {
        gridData = placementSystem.GridInfo;
        foreach (var pair in SaveLoadManager.Data.AreaAuthority)
        {
            if (pair.Value)
            {
                fogParticles[pair.Key - 1].gameObject.SetActive(false);
            }
        }

        UpdateLockIcon();
    }

    public void UnlockArea(int areaId)
    {
        SaveLoadManager.Data.AreaAuthority[areaId] = true;
        fogParticles[areaId - 1].Stop();
        fogParticles[areaId - 1].transform.parent.GetComponent<Renderer>().material = originalFloorMaterial;

        indicatorImage.material.DOFade(0, moveDuration);

        var data = areaRequirementDatabase.Get(areaId);
        SaveLoadManager.Data.inventory.RemoveItem(data.needItemId1, data.requiredCount1);
        SaveLoadManager.Data.inventory.RemoveItem(data.needItemId2, data.requiredCount2);
        SaveLoadManager.Data.inventory.RemoveItem(data.needItemId3, data.requiredCount3);
        SaveLoadManager.Data.Gold -= data.requiredGold;
        if (lockIcons.ContainsKey(areaId))
        {
            uiManager.iconAnimator.DisablePopupIcon(lockIcons[areaId]);
            lockIcons.Remove(areaId);
        }

        UpdateLockIcon();
    }

    private void ShowUnlockRequirementsUI(int areaId)
    {
        placementSystem.IsTouchable = false;
        Vector3 groundPivot = groundList[areaId - 1].position;
        Vector3 targetCameraPos = groundPivot + CameraOffset;
        SetIndicator(new Vector3(groundPivot.x, 0, groundPivot.z));
        Sequence sequence = DOTween.Sequence();
        sequence.Append(Camera.main.transform.DOMove(targetCameraPos, moveDuration));
        sequence.AppendCallback(() =>
        {
            areaExpansionUI.gameObject.SetActive(true);
            areaExpansionUI.Init(areaId, areaRequirementDatabase.Get(areaId), UnlockArea, OnUIClosed);
        });
        DOTween.To(() => Camera.main.orthographicSize, x => Camera.main.orthographicSize = x, 5f, moveDuration);
    }

    private void SetIndicator(Vector3 worldPos)
    {
        indicatorImage.gameObject.SetActive(true);
        indicatorImage.transform.position = worldPos;
        indicatorImage.material.DOFade(1, moveDuration);
    }

    public void OnTilePressed(Vector3Int pos)
    {
        int areaId = pos.GetAreaNumber();
        if (SaveLoadManager.Data.AreaAuthority.ContainsKey(areaId) && IsAdjacentTile(areaId) &&
            areaRequirementDatabase.Get(areaId).requiredLevel <= SaveLoadManager.Data.Level)
        {
            ShowUnlockRequirementsUI(areaId);
        }
    }

    public void OnUIClosed()
    {
        indicatorImage.material.DOFade(0, moveDuration);
        placementSystem.IsTouchable = true;
    }

    public bool IsAdjacentTile(int areaId)
    {
        bool isAdjacent = false;
        var authorityDict = SaveLoadManager.Data.AreaAuthority;
        var leftTopTile = areaId + Consts.xAxisAreaCount;
        if (authorityDict.ContainsKey(leftTopTile) && authorityDict[leftTopTile])
            isAdjacent = true;

        var rightTopTile = areaId + 1;
        if (authorityDict.ContainsKey(rightTopTile) && authorityDict[rightTopTile])
            isAdjacent = true;

        var leftBottomTile = areaId - 1;
        if (authorityDict.ContainsKey(leftBottomTile) && authorityDict[leftBottomTile])
            isAdjacent = true;

        var rightBottomTile = areaId - Consts.xAxisAreaCount;
        if (authorityDict.ContainsKey(rightBottomTile) && authorityDict[rightBottomTile])
            isAdjacent = true;

        return isAdjacent;
    }

    public void UpdateLockIcon()
    {
        var dict = SaveLoadManager.Data.AreaAuthority;
        Sprite icon = Resources.Load<Sprite>(lockIconPath);
        for (int i = 1; i <= dict.Count; i++)
        {
            if (!dict[i] && IsAdjacentTile(i) && !lockIcons.ContainsKey(i) &&
                areaRequirementDatabase.Get(i).requiredLevel <= SaveLoadManager.Data.Level)
            {
                lockIcons[i] =
                    uiManager.iconAnimator.PopupIconOnBuildingPos(icon,
                        groundList[i - 1].position + Vector3.up * iconYOffset);
            }
        }
    }
}