using UnityEngine;

public class Storage : MonoBehaviour, IBuilding
{
    private GameManager gameManager;
    private UiManager uiManager;
    private GameObject panel;

    public void OnTouch()
    {
        gameManager.PlacementSystem.IsTouchable = false;
        panel.SetActive(true);
    }

    public void Init(GameManager gameManager, UiManager uiManager, bool isFirst)
    {   
        this.gameManager = gameManager;
        this.uiManager = uiManager;
        panel = uiManager.GetPanel(MainSceneUiIds.Inventory);
    }
}
