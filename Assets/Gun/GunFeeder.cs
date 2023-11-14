using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunFeeder : GunPart, ISerializationCallbackReceiver
{

    public GunChamber chamber;
    public List<Ammunition> ammunition = new ();

    public List<GunHammer.HammerFeederLink> hammerFeederLinks;

    public void OnBeforeSerialize()
    {
        
    }

    public void OnAfterDeserialize()
    {
        GunFrame.ManualLinkGunParts<GunHammer.HammerFeederLink, GunHammer, GunFeeder>(hammerFeederLinks);
    }

    public void FeedRound(GunFrame.GunState gunState)
    {
        Debug.Log("Feeding round");
        if (ammunition.Count <= 0) return;
        var ammo = ammunition[0];
        chamber.Feed(ammo);
        ammunition.Remove(ammo);

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
