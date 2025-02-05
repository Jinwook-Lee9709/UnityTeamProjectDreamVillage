using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using Slider = UnityEngine.UI.Slider;

public class TimerBarUI : MonoBehaviour
{
    [SerializeField] private float yAxisOffset = 1000f;
    private float defaultPixel = 1080;
    
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private Slider slider;

    private StringBuilder sb = new();

    private DateTime plantedTime;
    private DateTime finishTime;

    private bool isActivatedOnFrame = false;
    
    public void SetInfo(DateTime plantedTime, DateTime finishTime, Vector3 position)
    {
        this.plantedTime = plantedTime;
        this.finishTime = finishTime;
        TimeSpan productTime = finishTime - plantedTime;
        TimeSpan remainTime = finishTime - DateTime.Now;
        TimeSpan proceededTime = DateTime.Now - plantedTime;

        float offset = Screen.dpi / defaultPixel * yAxisOffset;
        var screenPoint = Camera.main.WorldToScreenPoint(position);
        transform.position = new Vector3(screenPoint.x, screenPoint.y + offset, screenPoint.z);

        slider.value = (float)(proceededTime.TotalSeconds / productTime.TotalSeconds);
        timerText.text = MakeTimeString(remainTime);

        isActivatedOnFrame = true;
        ResetFlag();
    }

    private void Update()
    {
        if (HandleTouchEvent()) return;

        UpdateUI();
    }

    private bool HandleTouchEvent()
    {
        if (MultiTouchManager.Instance.Tap && !isActivatedOnFrame)
        {
            gameObject.SetActive(false);
            return true;
        }
        return false;
    }

    private void UpdateUI()
    {
        TimeSpan remainTime = finishTime - DateTime.Now;
        if (remainTime.Milliseconds < 0)
        {
            gameObject.SetActive(false);
        }
        TimeSpan productTime = finishTime - plantedTime;
        TimeSpan proceededTime = DateTime.Now - plantedTime;
        
        slider.value = (float)(proceededTime.TotalSeconds / productTime.TotalSeconds);
        timerText.text = MakeTimeString(remainTime);
    }

    private String MakeTimeString(TimeSpan timeSpan)
    {
        sb.Clear();
        if (timeSpan.Hours > 0)
            sb.Append($"{timeSpan.Hours}{DataTableManager.StringTable.Get("TIME_HOUR")} ");
        if(timeSpan.Minutes > 0)
            sb.Append($"{timeSpan.Minutes}{DataTableManager.StringTable.Get("TIME_MINUTE")} ");
        sb.Append($"{timeSpan.Seconds}{DataTableManager.StringTable.Get("TIME_SECOND")}");
        return sb.ToString();
    }

    private async UniTask ResetFlag()
    {
        await UniTask.WaitForEndOfFrame(this);
        isActivatedOnFrame = false;
    }
    
}
