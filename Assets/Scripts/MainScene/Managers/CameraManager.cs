using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public class CameraManager : MonoBehaviour
{
    [Flags]
    public enum CameraInput
    {
        None,
        DragMove,
        ScreenEdgeMove,
    }

    [SerializeField] private float minimumSize = 1.5f;
    [SerializeField] private float maximumSize = 5f;
    [SerializeField] private float pinchSensi = 1f;
    [SerializeField] private float dragSensi = 1f;
    [SerializeField] private float edgeMoveSensi = 1f;
    [SerializeField] private float edgeRatio = 0.1f;
    [SerializeField] private PlacementSystem placementSystem;
    [SerializeField] private Camera uiCamera;
    private Camera mainCamera;

    [SerializeField] private CameraInput currentCameraInput = 0;

    private int screenWidth = 0;
    private int screenHeight = 0;
    private Vector2 screenCenter = new();
    private float screenAspectRatio = 0;

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
        float newCameraSize = mainCamera.orthographicSize -= MultiTouchManager.Instance.Pinch;
        mainCamera.orthographicSize = Mathf.Clamp(newCameraSize, minimumSize, maximumSize);

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

    private void UpdateDragMove()
    {
        if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
            return;
        Vector2 dragVector = MultiTouchManager.Instance.Drag;
        Vector3 moveVector = -dragVector.ScreenVectorToWorldVector();
        mainCamera.transform.position += dragSensi * moveVector;
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
                mainCamera.transform.position += edgeMoveSensi * dir;
            }

            return true;
        }

        return false;
    }

    private void LateUpdate()
    {
        uiCamera.orthographicSize = mainCamera.orthographicSize;
    }
}