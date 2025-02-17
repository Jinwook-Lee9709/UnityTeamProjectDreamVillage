using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainSettingUI : MonoBehaviour
{
    [SerializeField] private Button saveButton;
    [SerializeField] private Button exitButton;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private GameObject mainPanel;
    [SerializeField] private Image backgroundImage;
    [SerializeField] private PlacementSystem placementSystem;
    public void Start()
    {
        float musicVol = SoundManager.Instance.MusicVolume;
        musicSlider.value = musicVol != 0 ? Mathf.Pow(10, musicVol / 20) : 1;
        float sfxVol = SoundManager.Instance.SfxVolume;
        sfxSlider.value = sfxVol != 0 ? Mathf.Pow(10, sfxVol / 20) : 1;
    }
    
    public void OnSaveButtonTouched()
    {
        SaveLoadManager.Save();
    }

    public void OnExitButtonTouched()
    {
        Application.Quit();
    }

    public void OnPopup()
    {
        gameObject.SetActive(true);
        DotAnimator.DissolveInAnimation(backgroundImage, alpha:0.7F);
        DotAnimator.PopupAnimation(mainPanel);
        SoundManager.Instance.PlaySfxByName(AudioNames.Popup.ToString());
        placementSystem.IsTouchable = false;
    }
    
    public void OnClose()
    {
        DotAnimator.DissolveOutAnimation(backgroundImage);
        DotAnimator.CloseAnimation(mainPanel, onComplete: () => { gameObject.SetActive(false); });
        SoundManager.Instance.PlaySfxByName(AudioNames.Close.ToString());
        placementSystem.IsTouchable = true;
    }
    

    public void SetMusicVolume(float volume)
    {
        SoundManager.Instance.MusicVolume = volume;
    }

    public void SetSfxVolume(float volume)
    {
        SoundManager.Instance.SfxVolume = volume;
    }


}
