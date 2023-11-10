using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunBarrel : GunPart
{
    public Transform firingLocation;
    public BarrelEvent fed = (shot, location) => {};
    public BarrelEvent emptied = (shot, location) => {};
    public BarrelEvent jammed = (shot, location) => {};
    
    public delegate void BarrelEvent(GunFrame.Shot shot, Transform barrelLocation);

    public BarrelEvent GetEvent(BarrelEventType barrelEventType) => barrelEventType switch
    {
        BarrelEventType.Emptied => emptied,
        BarrelEventType.Fed => fed,
        BarrelEventType.Jammed => jammed,
        _ => throw new ArgumentException()
    };

    public void FeedRound(GunFrame.Shot shot, Ammunition ammunition, GunFrame.GunState gunState)
    {
        fed(shot, firingLocation);
        //check gunstate
        emptied(shot, firingLocation);
    }

    [Serializable]
    public enum BarrelEventType
    {
        Fed,
        Emptied,
        Jammed
    }

    [Serializable]
    public struct BarrelBulletFactoryLink
    {
        public GunBarrel gunBarrel;
        public BulletFactory bulletFactory;

        public void Link()
        {
            if (!gunBarrel || !bulletFactory) return;
            gunBarrel.emptied += bulletFactory.CreateBullets;
        }

        public void UnLink()
        {
            if (!gunBarrel || !bulletFactory) return;
            gunBarrel.emptied -= bulletFactory.CreateBullets;
        }
    }
}
