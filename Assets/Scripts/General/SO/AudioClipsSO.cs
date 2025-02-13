using System;
using System.Collections;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEngine;

[CreateAssetMenu(fileName = "AudioClipDatabase", menuName = "ScriptableObjects/AudioClipDatabase")]
public class AudioClipDatabase : ScriptableObject
{
    [SerializedDictionary, SerializeField] SerializedDictionary<String, AudioClip> audioClipDictionary;

    public AudioClip Get(String name)
    {
        audioClipDictionary.TryGetValue(name, out AudioClip audioClip);
        return audioClip;
    }
}
