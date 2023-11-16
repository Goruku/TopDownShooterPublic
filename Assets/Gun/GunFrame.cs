using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class GunFrame : Entity, ISerializationCallbackReceiver
{
    public Actor owner;
    public OwnerAction ownerWillChange = actor => {};
    public OwnerAction ownerChanged = actor => {};
    
    public GunManagementType gunManagementType;
    
    public List<GunTrigger.TriggerHammerLink> triggerHammerLinks = new ();
    public List<GunHammer.HammerChamberLink> hammerChamberLinks = new();
    public List<GunChamber.ChamberBarrelLink> chamberBarrelLinks = new ();
    public List<GunBarrel.BarrelBulletFactoryLink> barrelBulletFactoryLinks = new ();
    
    public List<GunTrigger> triggers = new ();
    public List<GunHammer> hammers = new();
    public List<PropellantTank> propellantTanks = new ();
    public List<GunChamber> chambers = new ();
    public List<GunBarrel> barrels = new ();
    public List<BulletFactory> bulletFactories = new ();
    public List<GunPart> gunParts = new ();
    
    private GunState gunState = new GunState();

    private void OnTransformParentChanged()
    {
        ownerWillChange(owner);
        Entity.BindToClosest(transform, out owner);
        ownerChanged(owner);
    }

    public GunState GetState()
    {
        return gunState;
    }
    
    public void OnBeforeSerialize()
    {
        
    }

    private void AutoAfterSerialize()
    {
        AutoLinkGunParts(triggerHammerLinks, triggers, hammers);
        AutoLinkGunParts(hammerChamberLinks, hammers, chambers);
        AutoLinkGunParts(chamberBarrelLinks, chambers, barrels);
        AutoLinkGunParts(barrelBulletFactoryLinks, barrels, bulletFactories);
    }

    public static void AutoLinkGunParts<TGunLink, TGunPartActed, TGunPartReacting>(in List<TGunLink> gunLinks, in List<TGunPartActed> gunPartActing, in List<TGunPartReacting> gunPartReacted)
        where TGunLink : GunLink<TGunPartActed, TGunPartReacting>, new()
        where TGunPartActed : GunPart
        where TGunPartReacting : GunPart
    {
        foreach (var link in gunLinks)
        {
            link.UnLink();
        }
        gunLinks.Clear();
        var lowestCount = gunPartActing.Count <= gunPartReacted.Count
            ? gunPartActing.Count
            : gunPartReacted.Count;
        for (var i = 0; i < lowestCount; i++)
        {
            gunLinks.Add(new TGunLink
            {
                active = true,
                gunPartActed = gunPartActing[i],
                gunPartReacting = gunPartReacted[i]
            });
            gunLinks[i].Link();
        }
    }
    
    private void ManualAfterSerialize()
    {
        ManualLinkGunParts<GunTrigger.TriggerHammerLink, GunTrigger, GunHammer>(triggerHammerLinks);
        ManualLinkGunParts<GunHammer.HammerChamberLink, GunHammer, GunChamber>(hammerChamberLinks);
        ManualLinkGunParts<GunChamber.ChamberBarrelLink, GunChamber, GunBarrel>(chamberBarrelLinks);
        ManualLinkGunParts<GunBarrel.BarrelBulletFactoryLink, GunBarrel, BulletFactory>(barrelBulletFactoryLinks);
    }

    public static void ManualLinkGunParts<TGunLink, TGunPartActed, TGunPartReacting>(in List<TGunLink> gunLinks)
        where TGunLink : GunLink<TGunPartActed, TGunPartReacting>, new()
        where TGunPartActed : GunPart
        where TGunPartReacting : GunPart
    {
        foreach (var link in gunLinks)
        {
            link.UnLink();
            if (!link.active) continue;
            link.Link();
        }
    }

    public void OnAfterDeserialize()
    {
        if (gunManagementType == GunManagementType.Auto)
            AutoAfterSerialize();
        if (gunManagementType == GunManagementType.Manual)
            ManualAfterSerialize();
    }
    
    public struct GunState
    {
        
    }
    
    public struct Shot
    {
        public bool misfire;
        public bool empty;
        public bool jam;
        public List<GameObject> bullets;
        public float velocity;
        public float randomness;
        public AudioClip shotSound;
    }

    public enum GunManagementType
    {
        Auto,
        Manual
    }

    public delegate void OwnerAction(Actor actor);

    public abstract class GunLink<TGunPartActed, TGunPartReacting>
    where TGunPartActed : GunPart
    where TGunPartReacting : GunPart
    {
        public bool active;
        public TGunPartActed gunPartActed;
        public TGunPartReacting gunPartReacting;

        public abstract void Link();
        public abstract void UnLink();
    }
}
