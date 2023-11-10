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

    public List<GunPart> gunParts;

    public struct Shot
    {
        public bool misfire;
        public bool empty;
        public bool jam;
        public List<GameObject> bullets;
        public float velocity;
    }
    
    public float CalculateBulletVelocity(Ammunition ammunition)
    {
        float externalPush = 0;
        if (externalPropellant)
            externalPush = externalPropellant.EffectivePush();
        var individualPush = (externalPush + ammunition.propellant.EffectivePush()) / ammunition.bullets.Count;
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
            velocity = CalculateBulletVelocity(ammunition)
        };
        magazine.Remove(ammunition);
        return shot;
    }
}
