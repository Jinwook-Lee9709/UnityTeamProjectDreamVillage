using System;
using System.Collections;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class CameraManager : MonoBehaviour
{
    [Flags]
    public enum CameraInput
    {
        None,
        DragMove,
        ScreenEdgeMove,
    }

    public enum Direction
    {
        left,
        right,
        top,
        bottom,
    }

    [SerializeField] private float minimumSize = 1.5f;
    [SerializeField] private float maximumSize = 5f;

    [SerializeField] private float pinchSensi = 1f;
    [SerializeField] private float dragSensi = 1f;
    [SerializeField] private float edgeMoveSensi = 1f;
    [SerializeField] private float edgeRatio = 0.1f;

    [SerializedDictionary, SerializeField] private SerializedDictionary<Direction, Transform> restrictionBorderPivots;

    [SerializeField] private PlacementSystem placementSystem;
    [SerializeField] private Camera uiCamera;
    private Camera mainCamera;

    [SerializeField] private CameraInput currentCameraInput = 0;

    private int screenWidth = 0;
    private int screenHeight = 0;
    private Vector2 screenCenter = new();
    private float screenAspectRatio = 0;
    private float restrictRatio;
    private float moveRatioByZoom = 1;
    private float currentZoomScale = 1;

    private Vector3 verticalWorldVector;
    private Vector3 horizontalWorldVector;

    private Dictionary<Direction, Vector3> restrictionCoords = new();

    public bool DragMove
    {
        get => currentCameraInput.HasFlag(CameraInput.DragMove);
        set => currentCameraInput =
            value ? currentCameraInput | CameraInput.DragMove : currentCameraInput & ~CameraInput.DragMove;
    }

    public bool ScreenEdgeMove
    {
        get => currentCameraInput.HasFlag(CameraInput.ScreenEdgeMove);
        set => currentCameraInput =
            value ? currentCameraInput | CameraInput.ScreenEdgeMove : currentCameraInput & ~CameraInput.ScreenEdgeMove;
    }

    private void Awake()
    {
        CalculateScreenInfo();
        horizontalWorldVector = new Vector3(1, 0, -1).normalized;
        verticalWorldVector = new Vector3(1, 0, 1).normalized;
    }

    private void CalculateScreenInfo()
    {
        screenWidth = Screen.width;
        screenHeight = Screen.height;
        screenCenter = new Vector2(Screen.width / 2, Screen.height / 2);
        screenAspectRatio = (float)Screen.width / Screen.height;
    }

    private void Start()
    {
        mainCamera = Camera.main;
        currentCameraInput = CameraInput.DragMove;
    }

    private void Update()
    {
        if (Input.touchCount == 0)
            return;
        if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
            return;
        UpdateZoom();

        if (currentCameraInput.HasFlag(CameraInput.ScreenEdgeMove))
        {
            bool isMoved = UpdateEdgeMove();
            if (isMoved)
                return;
        }

        if (currentCameraInput.HasFlag(CameraInput.DragMove))
        {
            UpdateDragMove();
        }
    }

    private void UpdateZoom()
    {
        float newCameraSize = mainCamera.orthographicSize -= MultiTouchManager.Instance.Pinch;
        mainCamera.orthographicSize = Mathf.Clamp(newCameraSize, minimumSize, maximumSize);
        moveRatioByZoom = mainCamera.orthographicSize / 3f;
        CalculateRestrictBounds();
    }

    private void UpdateDragMove()
    {
        if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
            return;
        Vector2 dragVector = MultiTouchManager.Instance.Drag;
        Vector3 moveVector = -dragVector.ScreenVectorToWorldVector();
        var newPos = mainCamera.transform.position + dragSensi * moveRatioByZoom * moveVector;
        var clampedPos = ClampCoord(newPos);
        mainCamera.transform.position = clampedPos;

    }

    private bool UpdateEdgeMove()
    {
        if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
            return false;
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);
            Vector2 touchPos = touch.position;
            float widthThreshold = screenWidth * edgeRatio;
            float heightThreshold = screenHeight * edgeRatio;
            if (touchPos.x < widthThreshold || touchPos.x > screenWidth - widthThreshold
                                            || touchPos.y < heightThreshold * screenAspectRatio || touchPos.y >
                                            screenHeight - heightThreshold * screenAspectRatio)
            {
                var vec = touchPos - screenCenter;
                var dir = vec.normalized.ScreenVectorToWorldVector();
                var newPos = mainCamera.transform.position + edgeMoveSensi * moveRatioByZoom * dir;
                var clampedPos = ClampCoord(newPos);
                mainCamera.transform.position = clampedPos;
            }

            return true;
        }

        return false;
    }

    private void CalculateRestrictBounds()
    {
        Vector3 centerPos = mainCamera.ScreenToWorldPoint(screenCenter);
        Vector3 leftEndPos = mainCamera.ScreenToWorldPoint(new Vector2(0f, screenCenter.y));
        Vector3 bottomEndPos = mainCamera.ScreenToWorldPoint(new Vector2(screenCenter.x, 9));

        float horizontalMagnitude = (centerPos - leftEndPos).magnitude;
        float verticalMagnitude = (centerPos - bottomEndPos).magnitude;
  
        restrictionCoords[Direction.left] = (restrictionBorderPivots[Direction.left].position +
                                             horizontalWorldVector * horizontalMagnitude);
        restrictionCoords[Direction.right] = (restrictionBorderPivots[Direction.right].position -
                                              horizontalWorldVector * horizontalMagnitude);
        restrictionCoords[Direction.bottom] =
            (restrictionBorderPivots[Direction.bottom].position + verticalWorldVector * verticalMagnitude);
        restrictionCoords[Direction.top] =
            (restrictionBorderPivots[Direction.top].position - verticalWorldVector * verticalMagnitude);
    }

    private Vector3 ClampCoord(Vector3 point)
    {
        Vector2 leftPos = new Vector2 (restrictionCoords[Direction.left].x, restrictionCoords[Direction.left].z);
        Vector2 rightPos = new Vector2 (restrictionCoords[Direction.right].x, restrictionCoords[Direction.right].z);
        Vector2 bottomPos = new Vector2 (restrictionCoords[Direction.bottom].x, restrictionCoords[Direction.bottom].z);
        Vector2 topPos = new Vector2 (restrictionCoords[Direction.top].x, restrictionCoords[Direction.top].z);
        Vector2 newPos = new Vector2 (point.x, point.z); 
        
        Vector2 centerPos = (bottomPos + topPos) / 2;

        Vector2 rotatedLeftPos = Utils.RotatePoint(leftPos, centerPos, 45f);
        Vector2 rotatedRightPos = Utils.RotatePoint(rightPos, centerPos, 45f);
        Vector2 rotatedBottomPos = Utils.RotatePoint(bottomPos, centerPos, 45f);
        Vector2 rotatedTopPos = Utils.RotatePoint(topPos, centerPos, 45f);
        Vector2 rotatedNewPos = Utils.RotatePoint(newPos, centerPos, 45f);
        
        rotatedNewPos.x = Mathf.Clamp(rotatedNewPos.x, rotatedLeftPos.x, rotatedRightPos.x);
        rotatedNewPos.y = Mathf.Clamp(rotatedNewPos.y, rotatedBottomPos.y, rotatedTopPos.y);
        
        Vector2 invertedPos = Utils.RotatePoint(rotatedNewPos, centerPos, -45f);
        return new Vector3(invertedPos.x, point.y, invertedPos.y);
    }

    private void LateUpdate()
    { 
        uiCamera.orthographicSize = mainCamera.orthographicSize;
    }
}