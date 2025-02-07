using UnityEngine;

public class PreviewSystem : MonoBehaviour
{
    [SerializeField] private GameObject previewParent;
    [SerializeField] private GameObject gridVisualization;
    [SerializeField] private GameObject cursorIndicator;
    [SerializeField] private Transform cursorScale;
    [SerializeField] private Transform cursorRotation;
    [SerializeField] private Renderer cellIndicatorRenderer;
    private GameObject previewObject;

    private bool flip = false;
    public bool IsFlip { get => flip; }

    public Vector3 currentPreviewPosition
    {
        get => previewObject.transform.position;
    }

    private float previewYOffset = 0.1f;

    public void ShowPlacementPreview(GameObject prefab, Vector3 position, bool isValid)
    {
        previewObject = Instantiate(prefab, previewParent.transform);
        previewObject.transform.position = new Vector3(position.x, previewYOffset, position.z);
        previewObject.transform.rotation = Quaternion.identity;
        
        gridVisualization.SetActive(true);
        
        Vector2 buildingSize= prefab.GetComponent<BuildingSize>().size;
        cursorIndicator.transform.position = new Vector3(position.x, 0f, position.z);
        cursorRotation.rotation = Quaternion.identity;
        cursorScale.localScale = new Vector3(buildingSize.x, 1f, buildingSize.y);
        cellIndicatorRenderer.material.mainTextureScale = new Vector2(buildingSize.x, buildingSize.y);
        
        cursorIndicator.SetActive(true);
        
        UpdatePreview(isValid);
    }

    public void RotatePreview()
    {
        if (flip)
        {
            previewObject.transform.GetChild(0).transform.Rotate(Vector3.up, -90f);
            cursorRotation.Rotate(Vector3.up, -90f);
            flip = false;
        }
        else
        {
            previewObject.transform.GetChild(0).transform.Rotate(Vector3.up, 90f);
            cursorRotation.Rotate(Vector3.up, 90f);
            flip = true;
        }
    }

    public void MovePreviewObject(Vector3 position)
    {
        previewObject.transform.position = new Vector3(position.x, previewYOffset, position.z);
        cursorIndicator.transform.position = new Vector3(position.x, 0f, position.z);
    }
    public void UpdatePreview(bool isValid = true)
    {
        if (isValid)
        {
            foreach (var renderer in previewObject.GetComponentsInChildren<Renderer>())
            {
                renderer.material.color = Color.green;
            }
            cellIndicatorRenderer.material.color = Color.green;
        }
        else
        {
            foreach (var renderer in previewObject.GetComponentsInChildren<Renderer>())
            {
                renderer.material.color = Color.red;
            }
            cellIndicatorRenderer.material.color = Color.red;
        }
    }

    public void EndPlacementPreview()
    {
        Destroy(previewObject);
        gridVisualization.SetActive(false);
        cursorIndicator.SetActive(false);
        flip = false;
    }
}
