using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleSettingUI : MonoBehaviour
{
     [SerializeField] Slider musicSlider;
     [SerializeField] Slider sfxSlider;
     [SerializeField] private Button resetSaveButton;
     
     public void Start()
     {
         float musicVol = SoundManager.Instance.MusicVolume;
         musicSlider.value = musicVol != 0 ? Mathf.Pow(10, musicVol / 20) : 1;
         float sfxVol = SoundManager.Instance.SfxVolume;
         sfxSlider.value = sfxVol != 0 ? Mathf.Pow(10, sfxVol / 20) : 1;
     }

     public void OnEnable()
     {
         resetSaveButton.interactable = SaveLoadManager.IsSaveExists();
     }

     public void OnSaveReset()
     {
         SaveLoadManager.DeleteSave();
         resetSaveButton.interactable = SaveLoadManager.IsSaveExists();
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

