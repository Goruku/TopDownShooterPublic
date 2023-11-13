using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class GunChamber : GunPart
{
    public Propellant externalPropellant;
    public Caliber caliber;
    public Ammunition round;
    public AudioClip fireSound;
    public AudioClip emptySound;
    public AudioClip jam;
    public GunRandomness gunRandomness = new GunRandomness {gunRandomness = 0, gunIncidence = 0.5f};

    public ChamberEvent fed = (shot, ammunition, state) => {};
    public ChamberEvent fire = (shot, ammunition, state) => {Debug.Log("Fired");};
    public ChamberEvent wasEmpty = (shot, ammunition, state) => {Debug.Log("Empty");};
    public ChamberEvent jammed = (shot, ammunition, state) => {};

    public List<GunPart> gunParts;

    [Serializable]
    public struct GunRandomness
    {
        public float gunRandomness;
        public float gunIncidence;
    }

    public ChamberEvent GetEvent(ChamberEventType chamberEventType) => chamberEventType switch
    {
        ChamberEventType.Emptied => fire,
        ChamberEventType.Fed => fed,
        ChamberEventType.Jammed => jammed,
        _ => throw new ArgumentException()
    };
    
    public float CalculateBulletVelocity(Ammunition ammunition)
    {
        float push = 0;
        if (externalPropellant)
            push = externalPropellant.EffectivePush();
        if (ammunition.propellant)
            push += ammunition.propellant.EffectivePush();
        var individualPush = push / ammunition.bullets.Count;
        var scaledPush = individualPush * velocityMultiplier;
        foreach (var gunPart in gunParts)
        {
            scaledPush *= gunPart.velocityMultiplier;
        }
        return scaledPush;
    }

    public List<GameObject> GetBullets(Ammunition ammunition)
    {
        return ammunition.bullets;
    }

    public void ConsumeNextShot(GunFrame.GunState gunState)
    {
        if (!round)
        {
            wasEmpty(new GunFrame.Shot {empty = true, shotSound = emptySound}, round, gunFrame.GetState());
            return;
        }
        var shot = new GunFrame.Shot
        {
            misfire = round.caliber != caliber,
            jam = false,
            bullets = GetBullets(round),
            velocity = CalculateBulletVelocity(round),
            randomness = (gunRandomness.gunIncidence)*gunRandomness.gunRandomness + (1- gunRandomness.gunIncidence)*round.bulletRandomness,
            shotSound = fireSound
        };
        fire(shot, round, gunFrame.GetState());
        round = null;
    }

    public delegate void ChamberEvent(GunFrame.Shot shot, Ammunition ammunition, GunFrame.GunState gunState);

    [Serializable]
    public enum ChamberEventType
    {
        Fed,
        Emptied,
        Jammed
    }

    [Serializable]
    public class ChamberBarrelLink : GunFrame.GunLink<GunChamber, GunBarrel>
    {
        public override void Link()
        {
            if (!gunPartActed || !gunPartReacting) return;
            gunPartActed.fire += gunPartReacting.FeedRound;
        }

        public override void UnLink()
        {
            if (!gunPartActed || !gunPartReacting) return;
            gunPartActed.fire -= gunPartReacting.FeedRound;
        }
    }
}
