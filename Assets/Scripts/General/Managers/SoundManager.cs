using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

public class SoundManager : Singleton<SoundManager>
{
    public enum AudioType
    {
        Master,
        Music,
        Sfx,
    }
    [SerializeField] private AudioMixer audioMixer;
    
    private AudioSource musicSource;
    private AudioSource sfxSource;
    private AudioClipDatabase audioClipDatabase;
    
    public float MusicVolume
    {
        get
        {
            audioMixer.GetFloat(AudioType.Music.ToString(), out float vol);
            return vol;
        } 
        set
        {
            SetMusicVolume(value);
        }
    }
    
    public float SfxVolume
    {
        get
        {
            audioMixer.GetFloat(AudioType.Sfx.ToString(), out float vol);
            return vol;
        } 
        set
        {
            SetSfxVolume(value);
        }
    }


    private bool IsMuted = false;
    public void Awake()
    {
        audioMixer = Resources.Load<AudioMixer>("Audio/MasterMixer");
        SceneManager.sceneLoaded += (scene, mode) => FindAudioSource();
        audioClipDatabase = Resources.Load<AudioClipDatabase>(String.Format(PathFormat.soPath, nameof(AudioClipDatabase)));
        FindAudioSource();
    }
    
    private void FindAudioSource()
    {
        var sources = Camera.main.GetComponents<AudioSource>();
        foreach (var source in sources)
        {
            if (source.outputAudioMixerGroup.name == AudioType.Music.ToString())
            {
                musicSource = source;
            }

            if (source.outputAudioMixerGroup.name == AudioType.Sfx.ToString())
            {
                sfxSource = source;
            }
        }
    }
    
    
    public void SetMusicVolume(float volume)
    {
        SetVolume(AudioType.Music, volume);
    }

    public void SetSfxVolume(float volume)
    {
        SetVolume(AudioType.Sfx, volume);
    }
    
    public void SetVolume(AudioType type, float volume)
    {
        if(!IsMuted)
            audioMixer.SetFloat(type.ToString(), volume !=0 ? Mathf.Log10(volume) * 20 : -60);
    }

    public void PlayMusic(AudioClip clip)
    {
        musicSource.clip = clip;
        musicSource.Play();
    }

    public void PlaySfx(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }

    public void PlaySfxByName(String name)
    {
        sfxSource.PlayOneShot(audioClipDatabase.Get(name));
    }
    
    
}