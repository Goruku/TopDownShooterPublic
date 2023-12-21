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
    public List<AudioChannel> audioChannels = new ();
    public float defaultLocalSound = 1;

    public bool shouldFetchSettings = false;

    private void Start()
    {
        UpdateAllVolume();
    }

    private void Reset()
    {
        var localAudioSource = GetComponent<AudioSource>();
        if (localAudioSource)
            GetChannels().Add(new AudioChannel(){audioSource = localAudioSource, localSound = defaultLocalSound}); 
    }

    protected virtual void FixedUpdate()
    {
        if (localSettingHash != audioSettings.settingHash)
            UpdateAllVolume();
    }

    public void PlayOneShotMastered(AudioClip audioClip, float volumeScale=1f, int index=0)
    {
        if (localSettingHash != audioSettings.settingHash)
            UpdateAllVolume();
        audioChannels[index].PlayOneShot(audioClip, volumeScale);
    }
    
    public void UpdateAllVolume()
    {
        if (!audioSettings)
        {
            Debug.LogWarning("No AudioSettings Found");
            return;
        }

        foreach (var audioChannel in GetChannels())
        {
            audioChannel.UpdateVolume(audioSettings);
        }
        localSettingHash = audioSettings.settingHash;
        shouldFetchSettings = false;
    }

    protected virtual List<AudioChannel> GetChannels()
    {
        return audioChannels;
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
        public AudioSettings cachedAudioSettings;

        public bool neverSpatial;
        public AudioSource audioSource;
        public AudioSettings.Channel channel;
        private float _oldLocalSound;
        [Range(0f, 1f)]
        public float localSound;


        
        public void PlayOneShot(AudioClip audioClip , float volumeScale=1f)
        {
            if (localSound != _oldLocalSound)
                UpdateVolume();
            audioSource.PlayOneShot(audioClip, volumeScale);
        }

        public void UpdateVolume(AudioSettings audioSettings)
        {
            cachedAudioSettings = audioSettings;
            _oldLocalSound = localSound;
            audioSource.volume = audioSettings.GetVolume(channel) * localSound;
            audioSource.spatialBlend = neverSpatial ? 0 : audioSettings.spatialBlend;
        }

        public void UpdateVolume()
        {
            UpdateVolume(cachedAudioSettings);
        }
    }
}
