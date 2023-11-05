using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class GunFrame : MonoBehaviour
{
    public Propellant externalPropellant;
    public Caliber caliber;
    public List<Ammunition> magazine;
    public AudioClip fireSound;
    public AudioClip emptySound;
    public AudioClip jam;
    public float baseVelocityMultiplier;
    public GunRandomness gunRandomness = new GunRandomness {gunRandomness = 0, gunIncidence = 0.5f};

    [Serializable]
    public struct GunRandomness
    {
        public float gunRandomness;
        public float gunIncidence;
    }

    public List<GunPart> gunParts;

    public struct Shot
    {
        public bool misfire;
        public bool empty;
        public bool jam;
        public List<GameObject> bullets;
        public float velocity;
        public float randomness;
    }
    
    public float CalculateBulletVelocity(Ammunition ammunition)
    {
        float push = 0;
        if (externalPropellant)
            push = externalPropellant.EffectivePush();
        if (ammunition.propellant)
            push += ammunition.propellant.EffectivePush();
        var individualPush = push / ammunition.bullets.Count;
        var scaledPush = individualPush * baseVelocityMultiplier;
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

    public Shot ConsumeNextShot()
    {
        if (magazine.Count <= 0) return new Shot {empty = true};
        var ammunition = magazine[0];
        var shot = new Shot
        {
            misfire = ammunition.caliber != caliber,
            jam = false,
            bullets = GetBullets(ammunition),
            velocity = CalculateBulletVelocity(ammunition),
            randomness = (gunRandomness.gunIncidence)*gunRandomness.gunRandomness + (1- gunRandomness.gunIncidence)*ammunition.bulletRandomness
        };
        magazine.Remove(ammunition);
        return shot;
    }
}
