using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(AudioManager))]
[RequireComponent(typeof(GunFrame))]
public class GunSoundManager : MonoBehaviour
{
    [FormerlySerializedAs("gunSounds")] public GunSoundCollection gunSoundCollection;
    public AudioManager audioManager;

    public List<GunChamber> gunChambers;
    public List<GunHammer> gunHammers;

    private void Reset()
    {
        audioManager = GetComponent<AudioManager>();
    }

    void Start()
    {
        foreach (var gunChamber in gunChambers)
        {
            gunChamber.fire += ChamberEventHandlerWithAudio(gunSoundCollection.Firing);
            gunChamber.wasEmpty += ChamberEventHandlerWithAudio(gunSoundCollection.FiringEmpty);
            gunChamber.jammed += ChamberEventHandlerWithAudio(gunSoundCollection.Jammed);
        }

        foreach (var gunHammer in gunHammers)
        {
            gunHammer.pulled += HammerEventHandlerWithAudio(gunSoundCollection.Arming);
            gunHammer.released += HammerEventHandlerWithAudio(gunSoundCollection.ReleasingHammer);
        }
    }

    GunChamber.ChamberEvent ChamberEventHandlerWithAudio(GunSoundCollection.GunSound gunSound)
    {
        void ChamberEventHandler(GunFrame.Shot shot, Ammunition ammunition, GunFrame.GunState gunState)
        {
            //modifiy sound at will
            audioManager.PlayOneShotMastered(gunSound.audioClip, gunSound.soundScale);
        }

        return ChamberEventHandler;
    }

    GunHammer.HammerEvent HammerEventHandlerWithAudio(GunSoundCollection.GunSound gunSound)
    {
        void HammerEventHandler(GunFrame.GunState gunState)
        {
            audioManager.PlayOneShotMastered(gunSound.audioClip, gunSound.soundScale);
        }

        return HammerEventHandler;
    }

}
