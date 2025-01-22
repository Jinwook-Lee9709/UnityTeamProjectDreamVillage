using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreviewSystem : MonoBehaviour
{
    [SerializeField]
    private GameObject previewParent;
    private GameObject previewObject;

    private bool flip = false;
    public bool IsFlip { get => flip; }

    public Vector3 currentPreviewPosition
    {
        get => previewObject.transform.position;
    }

    private float previewYOffset = 0.1f;

    public void ShowPlacementPreview(GameObject prefab, Vector3 position)
    {
        previewObject = Instantiate(prefab, previewParent.transform);
        previewObject.transform.position = new Vector3(position.x, previewYOffset, position.z);
    }

    public void RotatePreview()
    {
        if (flip)
        {
            previewObject.transform.GetChild(0).transform.Rotate(Vector3.up, -90f);
            flip = false;
        }
        else
        {
            previewObject.transform.GetChild(0).transform.Rotate(Vector3.up, 90f);
            flip = true;
        }
    }

    public void MovePreviewObject(Vector3 position)
    {
        previewObject.transform.position = new Vector3(position.x, previewYOffset, position.z);
    }
    public void UpdatePreview(bool isValid = true)
    {
        if (isValid)
        {
            previewObject.GetComponentInChildren<Renderer>().material.color = Color.green;
        }
        else
        {
            previewObject.GetComponentInChildren<Renderer>().material.color = Color.red;
        }
    }

    public void EndPlacementPreview()
    {
        Destroy(previewObject);
        flip = false;
    }
}
