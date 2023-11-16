using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class GunFeeder : InteractibleGunPart, ISerializationCallbackReceiver
{

    public GunChamber chamber;
    public List<Ammunition> ammunition = new ();
    public List<Ammunition> defaultMagazine = new();
    
    protected override string DefaultPlayerAction
    {
        get => "Reload";
    }

    public List<GunHammer.HammerFeederLink> hammerFeederLinks;

    public void OnBeforeSerialize()
    {
        
    }

    public void OnAfterDeserialize()
    {
        GunFrame.ManualLinkGunParts<GunHammer.HammerFeederLink, GunHammer, GunFeeder>(hammerFeederLinks);
    }

    protected override void BindPerformed(InputAction.CallbackContext callbackContext)
    {
        ammunition.Clear();
        foreach (var ammo in defaultMagazine)
        {
            ammunition.Add(Instantiate(ammo));
        }
    }

    protected override void BindStarted(InputAction.CallbackContext callbackContext)
    {
    }

    protected override void BindCanceled(InputAction.CallbackContext callbackContext)
    {
        
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

    void Update()
    {
        
    }
}
