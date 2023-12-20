using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class MusicManager : AudioManager
{
    public AnimationCurve crossFadeInCurve;
    public AnimationCurve crossFadeOutCurve;

    public List<MusicEvent> activeEvents;

    public List<MusicChannel> musicChannels;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [Serializable]
    public class MusicChannel
    {
        public MusicEvent musicEvent;
        public AudioSource audioSource;
    }
    
    [Serializable]
    public enum MusicEvent {
        Base,
        Fight,
        Menu,
        LowLife
    }
}
