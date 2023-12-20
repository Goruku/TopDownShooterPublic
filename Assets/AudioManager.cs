using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

public class AudioManager : MonoBehaviour, ISerializationCallbackReceiver
{
    public uint localSettingHash = 0;
    private AudioSettings _oldAudioSettings;
    public AudioSettings audioSettings;
    public List<AudioChannel> audioChannels;
    public float defaultLocalSound = 1;

    public bool shouldFetchSettings = false;

    private void Reset()
    {
        var localAudioSource = GetComponent<AudioSource>();
        audioChannels.Add(new AudioChannel(){audioSource = localAudioSource, localSound = defaultLocalSound}); 
    }

    private void Update()
    {
        if (shouldFetchSettings)
            UpdateAllVolume();
    }

    public void PlayOneShotMastered(AudioClip audioClip, float volumeScale=1f, int index=0)
    {
        if (localSettingHash != audioSettings.settingHash)
            UpdateAllVolume();
        audioChannels[index].PlayOneShot(audioClip, volumeScale);
    }
    
    private void UpdateAllVolume()
    {
        if (!audioSettings)
        {
            Debug.LogWarning("No AudioSettings Found");
            return;
        }

        foreach (var audioChannel in audioChannels)
        {
            audioChannel.UpdateVolume(audioSettings);
        }
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

    [Serializable]
    public class AudioChannel
    {
        public AudioSource audioSource;
        public AudioSettings.Channel channel;
        [Range(0f, 1f)]
        public float localSound;

        public void PlayOneShot(AudioClip audioClip , float volumeScale=1f)
        {
            audioSource.PlayOneShot(audioClip, volumeScale * localSound);
        }

        public void UpdateVolume(AudioSettings audioSettings)
        {
            audioSource.volume = audioSettings.GetVolume(channel);
            audioSource.spatialBlend = audioSettings.spatialBlend;
        }
    }
}
