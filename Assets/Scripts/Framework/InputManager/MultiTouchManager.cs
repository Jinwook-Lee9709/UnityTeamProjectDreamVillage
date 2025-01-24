using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Timeline;

public class MultiTouchManager : Singleton<MultiTouchManager>
{
    private float tapInterval = 0.2f;
    private float doubleTapInterval = 0.2f;
    private float longPressTime = 0.6f;
    private float touchedTime = -1;
    private float doubleTapTime = -1;

    private int fingerId = -1;   
    private int fingerIdFirst = -1;
    private int fingerIdSecond = -1;
    
    private Vector2 fingerTouchStartPos;
    private float fingerTouchStartTime;
    public float minSwipeDistance = 0.1f;
    public float maxSwipeTime = 0.25f;
    private float minSwipeDistancePixels;
    
    
    
    public bool Tap { get; private set; }
    public bool DoubleTap { get; private set; }
    public bool LongPress { get; private set; }
    
    public Vector2 Swipe { get; private set; }
    
    public float Pinch { get; private set; }
    
    public float Rotate { get; private set; }

    private Vector2 tapPosition = Vector2.zero;
    public Vector2 lastTapPosition { get; private set; } = Vector2.zero;
    private void Start()
    {
        minSwipeDistancePixels = minSwipeDistance * Screen.dpi;
    }

    public void Update()
    {
        UpdateTap();
        UpdateSwipe();
        UpdatePinchAndRotate();
    }
    
    private void UpdatePinchAndRotate()
    {
        foreach (Touch touch in Input.touches)
        {
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    if(fingerIdFirst == -1)
                        fingerIdFirst = touch.fingerId;
                    else if(fingerIdSecond == -1)
                        fingerIdSecond = touch.fingerId;
                    break;
                case TouchPhase.Moved:
                case TouchPhase.Stationary:
                    break;
                case TouchPhase.Ended:
                case TouchPhase.Canceled:
                    if (fingerIdFirst == touch.fingerId)
                        fingerIdFirst = -1;
                    else if(fingerIdSecond == touch.fingerId)
                        fingerIdSecond = -1;
                    break;
            }
        }

        if (fingerIdFirst != -1 && fingerIdSecond != -1)
        {
            Touch touch0 = Input.touches.First(t => t.fingerId == fingerIdFirst);
            Touch touch1 = Input.touches.First(t => t.fingerId == fingerIdSecond);

            Vector2 prevTouch0Pos = touch0.position - touch0.deltaPosition;;
            Vector2 prevTouch1Pos = touch1.position - touch1.deltaPosition;;
            
            float prevTouchDistance = Vector2.Distance(prevTouch0Pos, prevTouch1Pos);
            float currentTouchDistance = Vector2.Distance(touch0.position, touch1.position);
            
            Pinch = (currentTouchDistance - prevTouchDistance) / Screen.dpi;

            Vector2 currentDir = touch1.position - touch0.position;
            Vector2 prevDir = prevTouch1Pos - prevTouch0Pos;

            Rotate = Vector2.SignedAngle(currentDir, prevDir);
        }
        else
        {
            Pinch = 0;
            Rotate = 0;
        }
    }

    private void UpdateSwipe()
    {
        foreach (Touch touch in Input.touches)
        {
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    if (fingerId == -1)
                    {
                        fingerId = touch.fingerId;
                        fingerTouchStartPos = touch.position;
                        fingerTouchStartTime = Time.time;
                    }

                    break;
                case TouchPhase.Moved:
                case TouchPhase.Stationary:
                    break;
                case TouchPhase.Ended:
                case TouchPhase.Canceled:
                    if (fingerId == touch.fingerId)
                    {
                        fingerId = -1;
                        var dir = touch.position - fingerTouchStartPos;
                        var distance = dir.magnitude;
                        var direction = (touch.position - fingerTouchStartPos).normalized;
                        if (Time.time - fingerTouchStartTime < maxSwipeTime &&
                            Mathf.Abs(distance) > minSwipeDistancePixels)
                        {
                            Swipe = dir.normalized;
                        }

                        fingerTouchStartPos = Vector2.zero;
                    }

                    break;
            }
        }
    }

    private void UpdateTap()
    {
        if (doubleTapTime < Time.time && doubleTapTime > 0)
        {
            Tap = true;
            lastTapPosition = tapPosition;
            doubleTapTime = -1;
        }

        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    fingerId = Input.touches[0].fingerId;
                    touchedTime = Time.time;
                    break;
                case TouchPhase.Moved:
                case TouchPhase.Stationary:
                    if (touchedTime + longPressTime < Time.time && touchedTime > 0)
                    {
                        LongPress = true;
                        touchedTime = -1;
                    }

                    break;
                case TouchPhase.Ended:
                case TouchPhase.Canceled:
                    if (doubleTapTime > Time.time)
                    {
                        DoubleTap = true;
                        doubleTapTime = -1;
                    }
                    else if (touchedTime + tapInterval > Time.time)
                    {
                        tapPosition = touch.position;
                        doubleTapTime = Time.time + doubleTapInterval;
                    }

                    touchedTime = -1;
                    fingerId = -1;
                    break;
            }
        }
    }

    private void LateUpdate()
    {
        Tap = false;
        DoubleTap = false;
        LongPress = false;

        Swipe = Vector2.zero;
        if (lastTapPosition != Vector2.zero)
        {
            lastTapPosition = Vector2.zero;
            tapPosition = lastTapPosition;
        }
    }
    
    private bool IsApproximately(Vector2 a, Vector2 b, float tolerance = 0.0001f)
    {
        return Mathf.Abs(a.x - b.x) < tolerance && Mathf.Abs(a.y - b.y) < tolerance;
    }
}

