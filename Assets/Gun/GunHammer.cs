using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GunHammer : InteractibleGunPart
{
    public HammerEvent struck = state => {};
    public HammerEvent pulled = state => {Debug.Log("Hammer Pulled");};
    public HammerEvent released = state => {Debug.Log("Hammer Released");};
    public HammerEvent performed = state => {};
    public HammerEvent started = state => {};
    public HammerEvent canceled = state => {};

    private bool isPulled = false;
    
    protected override string DefaultPlayerAction
    {
        get => "PullHammer";
    }

    private void BasePulled(GunFrame.GunState gunState)
    {
        isPulled = true;
    }

    public void CallPulledIfNotPulled(GunFrame.GunState gunState)
    {
        if (isPulled) return;
        pulled(gunState);
    }

    private void BaseReleased(GunFrame.GunState gunState)
    {
        isPulled = false;
    }

    private new void OnEnable()
    {
        base.OnEnable();
        pulled += BasePulled;
        released += BaseReleased;
    }

    private new void OnDisable()
    {
        base.OnEnable();
        pulled -= BasePulled;
        released -= BaseReleased;
    }

    protected override void BindPerformed(InputAction.CallbackContext callbackContext)
    {
        performed(gunFrame.GetState());
        if (isPulled)
            released(gunFrame.GetState());
        else
            pulled(gunFrame.GetState());
    }

    protected override void BindCanceled(InputAction.CallbackContext callbackContext)
    {
        canceled(gunFrame.GetState());
    }

    protected override void BindStarted(InputAction.CallbackContext callbackContext)
    {
        started(gunFrame.GetState());
    }

    public void AttemptStrike(GunFrame.GunState gunState)
    {
        if (!isPulled) return;
        struck(gunState);
        isPulled = false;
    }

    public delegate void HammerEvent(GunFrame.GunState gunState);
    
    [Serializable]
    public class HammerChamberLink : GunFrame.GunLink<GunHammer, GunChamber>
    {
        public override void Link()
        {
            if (!gunPartActed || !gunPartReacting) return;
            gunPartActed.struck += gunPartReacting.ConsumeNextShot;
        }

        public override void UnLink()
        {
            if (!gunPartActed || !gunPartReacting) return;
            gunPartActed.struck -= gunPartReacting.ConsumeNextShot;
        }
    }
}
