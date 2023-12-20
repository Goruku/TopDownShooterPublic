using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Serialization;

public class MusicManager : AudioManager, ISerializationCallbackReceiver
{

    private HashSet<MusicEvent> formerActiveEvents = new ();
    
    public List<MusicEvent> _activeEvents = new ();
    private HashSet<MusicEvent> activeEvents = new ();
    
    [FormerlySerializedAs("musicChannels")] [SerializeField]
    public List<MusicChannel> _musicChannels = new ();

    private Dictionary<MusicEvent, MusicChannel> musicChannels = new ();

    private List<Action> deferredFades = new ();
    
    public override List<AudioChannel> GetChannels()
    {
        return _musicChannels.Cast<AudioChannel>().ToList();
    }
    
    void FixedUpdate()
    {
        if (shouldFetchSettings)
            UpdateAllVolume();
        
        foreach (var deferredFade in deferredFades)
        {
            deferredFade();
        }
        
        deferredFades.Clear();
        
        foreach (var musicChannel in _musicChannels)
        {
            if (musicChannel.isFading)
            {
                musicChannel.Fade();
            }
        }
    }
    
    public new void OnBeforeSerialize()
    {
        base.OnBeforeSerialize();
        _musicChannels.Clear();
        foreach (var musicChannel in musicChannels.Values)
        {
            _musicChannels.Add(musicChannel);
        }
        
        _activeEvents.Clear();
        foreach (var activeEvent in activeEvents)
        {
            _activeEvents.Add(activeEvent);
        }
    }

    public new void OnAfterDeserialize()
    {
        musicChannels.Clear();
        foreach (var musicChannel in _musicChannels)
        {
            if (musicChannels.ContainsKey(musicChannel.musicEvent))
                musicChannel.musicEvent = MusicEvent.EDIT_ONLY;
            musicChannels.Add(musicChannel.musicEvent, musicChannel);
        }

        activeEvents.Clear();
        foreach (var activeEvent in _activeEvents)
        {
            if (activeEvents.Contains(activeEvent))
            {
                activeEvents.Add(MusicEvent.EDIT_ONLY);
            }
            else
            {
                activeEvents.Add(activeEvent);
            }
        }
        
        var addedEvents = activeEvents.Except(formerActiveEvents);
        var removedEvents = formerActiveEvents.Except(activeEvents);

        foreach (var addedEvent in addedEvents)
        {
            deferredFades.Add(musicChannels[addedEvent].InitiateFadeIn);
        }
        
        foreach (var removedEvent in removedEvents)
        {
            deferredFades.Add(musicChannels[removedEvent].InitiateFadeOut);
        }

        formerActiveEvents = new HashSet<MusicEvent>(activeEvents);
    }

    [Serializable]
    public class MusicChannel: AudioChannel
    {
        public MusicEvent musicEvent;
        
        public AnimationCurve crossFadeInCurve;
        public AnimationCurve crossFadeOutCurve;
        public float fadeInTime = 1f;
        public float fadeOutTime = 5;
        public bool isFading;
        public bool isFadingIn;
        public float fadeStartTime;
        
        public void InitiateFadeIn()
        {
            isFading = true;
            fadeStartTime = Time.time;
            isFadingIn = true;
        }

        public void Fade()
        {
            float fadeTime = isFadingIn ? fadeInTime : fadeOutTime;
            AnimationCurve fadeCurve = isFadingIn ? crossFadeInCurve : crossFadeOutCurve;


            localSound = fadeCurve.Evaluate(Mathf.Clamp(Time.time - fadeStartTime, 0, fadeTime)/fadeTime);
            UpdateVolume();
            
            //The fade has finished
            if (Time.time >= fadeStartTime + fadeTime)
            {
                isFading = false;
            }
        }

        public void InitiateFadeOut()
        {
            isFading = true;
            fadeStartTime = Time.time;
            isFadingIn = false;
        }
    }
    
    [Serializable]
    public enum MusicEvent {
        Base,
        Fight,
        Menu,
        LowLife,
        EDIT_ONLY
    }
}
