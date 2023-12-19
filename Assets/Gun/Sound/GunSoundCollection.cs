using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class GunSoundCollection : ScriptableObject
{
    public GunSound Reloading;
    public GunSound Chambering;
    public GunSound Arming;
    public GunSound ReleasingHammer;
    public GunSound PullingTrigger;
    public GunSound Firing;
    public GunSound FiringEmpty;
    public GunSound Jammed;

    [Serializable]
    public class GunSound
    {
        public AudioClip audioClip;
        [Range(0f,2f)]
        public float soundScale = 1;
    }
}
