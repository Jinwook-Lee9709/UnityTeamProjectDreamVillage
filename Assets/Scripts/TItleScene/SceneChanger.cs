using System;
using UnityEngine;
using System.Collections;
using Cysharp.Threading.Tasks;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    private const String percentForamt = "{0}%";
    private UniTask test;
    public void OnStartButtonTouched()
    {
        button.gameObject.SetActive(false);
        loadUI.gameObject.SetActive(true);
        LoadData();
    }
    
    private void OnDestroy()
    {
        if (test.Status == UniTaskStatus.Pending)
        {
            test.ToCancellationToken();
        }
    }

    private AsyncOperation async;

    [SerializeField] private GameObject loadUI;

    [SerializeField] private Slider slider;
    
    [SerializeField] private GameObject button;
    
    [SerializeField] private TextMeshProUGUI text;

    private async UniTask LoadData()
    {
        async = SceneManager.LoadSceneAsync(SceneIds.MainScene.ToString(), LoadSceneMode.Single);
        async.allowSceneActivation = false;
        UniTask loadTask = SaveLoadManager.Load();
        
        while (!async.isDone){
            var progressVal = Mathf.Clamp01(async.progress / 0.9f);
            slider.value = progressVal;
            text.text = String.Format(percentForamt, (progressVal * 100).ToString("F0"));
            Debug.Log(async.isDone);
            if(progressVal ==1)
                async.allowSceneActivation = true;
            await UniTask.Yield(PlayerLoopTiming.Update, cancellationToken: this.GetCancellationTokenOnDestroy());
        }

        await loadTask;
    }
}