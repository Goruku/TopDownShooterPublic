using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GunFrame : Entity, ISerializationCallbackReceiver
{
    public Actor owner;
    public OwnerAction ownerWillChange = actor => {};
    public OwnerAction ownerChanged = actor => {};
    
    public List<GunTrigger> triggers = new ();
    public List<PropellantTank> propellantTanks = new ();
    public List<GunChamber> chambers = new ();
    public List<GunBarrel> barrels = new ();
    public List<BulletFactory> bulletFactories = new ();
    public List<GunPart> gunParts = new ();

    public List<GunTrigger.TriggerChamberLink> triggerChamberLinks = new ();
    public List<GunChamber.ChamberBarrelLink> chamberBarrelLinks = new ();
    public List<GunBarrel.BarrelBulletFactoryLink> barrelBulletFactoryLinks = new ();

    public GunManagementType gunManagementType;
    
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
        foreach (var link in triggerChamberLinks)
        {
            link.UnLink();
        }
        triggerChamberLinks.Clear();
        var triggerChamberCount = triggers.Count <= chambers.Count
            ? triggers.Count
            : chambers.Count;
        for (var i = 0; i < triggerChamberCount; i++)
        {
            if (!triggers[i]) continue;
            triggerChamberLinks.Add(new GunTrigger.TriggerChamberLink
            {
                gunChamber = chambers[i],
                gunTrigger = triggers[i],
                triggerEventType = triggers[i].triggerEventType
            });
            triggerChamberLinks[i].Link();
        }

        foreach (var link in chamberBarrelLinks)
        {
            link.UnLink();
        }
        chamberBarrelLinks.Clear();
        var chamberBarrelCount = chambers.Count <= barrels.Count
            ? chambers.Count
            : barrels.Count;
        for (var i = 0; i < chamberBarrelCount; i++)
        {
            chamberBarrelLinks.Add(new GunChamber.ChamberBarrelLink
            {
                gunChamber = chambers[i],
                gunBarrel = barrels[i],
            });
            chamberBarrelLinks[i].Link();
        }
        
        foreach (var link in barrelBulletFactoryLinks)
        {
            link.UnLink();
        }
        barrelBulletFactoryLinks.Clear();
        var barrelFactoryCount = barrels.Count <= bulletFactories.Count
            ? barrels.Count
            : bulletFactories.Count;
        for (var i = 0; i < barrelFactoryCount; i++)
        {
            barrelBulletFactoryLinks.Add(new GunBarrel.BarrelBulletFactoryLink
            {
                gunBarrel = barrels[i],
                bulletFactory = bulletFactories[i]
            });
            barrelBulletFactoryLinks[i].Link();
        }
    }

    public void OnAfterDeserialize()
    {
        if (gunManagementType == GunManagementType.Auto)
            AutoAfterSerialize();
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
        Auto
    }

    public delegate void OwnerAction(Actor actor);
}
