using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour, ISerializationCallbackReceiver
{
    public uint localSettingHash = 0;
    private AudioSettings _oldAudioSettings;
    public AudioSettings audioSettings;
    public AudioSource audioSource;
    [Range(0f, 1f)]
    public float localSound = 1;

    public bool shouldFetchSettings = false;

    public AudioSettings.Channel channel;

    private void Reset()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (shouldFetchSettings)
            UpdateVolume();
    }

    public void PlayOneShotMastered(AudioClip audioClip, float volumeScale=1f)
    {
        if (localSettingHash != audioSettings.settingHash)
            UpdateVolume();
        audioSource.PlayOneShot(audioClip, volumeScale);
    }
    
    private void UpdateVolume()
    {
        if (!audioSettings)
        {
            Debug.LogWarning("No AudioSettings Found");
            return;
        }
        audioSource.volume = audioSettings.GetVolume(channel) * localSound;
        audioSource.spatialBlend = audioSettings.spatialBlend;
        localSettingHash = audioSettings.settingHash;
        shouldFetchSettings = false;
    }

    public void OnBeforeSerialize()
    {
        shouldFetchSettings = true;
    }

    public void OnAfterDeserialize()
    {

    }
}
