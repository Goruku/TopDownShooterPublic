using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class AudioSettings : ScriptableObject, ISerializationCallbackReceiver
{
    public uint settingHash = 0;
    [Range(0f, 1f)]
    public float spatialBlend = 1;

    [Range(0f, 1f)]
    public float masterVolume = 1;
    [Range(0f, 1f)]
    public float sfxVolume = 1;
    [Range(0f, 1f)]
    public float gunSoundsVolume = 1;
    [Range(0f, 1f)]
    public float environmentVolume = 1;
    [Range(0f, 1f)]
    public float footprintVolume = 1;
    [Range(0f, 1f)]
    public float musicVolume = 1;

    private float Volume(Channel channel) => channel switch
    {
        Channel.Sfx => sfxVolume,
        Channel.GunSounds => gunSoundsVolume,
        Channel.Environment => environmentVolume,
        Channel.Footprint => footprintVolume,
        Channel.Music => musicVolume,
        _ => 1,
    };
    
    public float GetVolume(Channel channel)
    {
        return masterVolume * Volume(channel);
    }
    
    public enum Channel
    {
        Master,
        Sfx,
        GunSounds,
        Environment,
        Footprint,
        Music
    }

    public void OnBeforeSerialize()
    {

    }

    public void OnAfterDeserialize()
    {
        settingHash++;
    }
}
