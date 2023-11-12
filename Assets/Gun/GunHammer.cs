using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GunHammer : InteractibleGunPart
{
    public HammerEvent struck = state => {};
    public HammerEvent pulled = state => {};
    public HammerEvent performed = state => {};
    public HammerEvent started = state => {};
    public HammerEvent canceled = state => {};

    protected override string DefaultPlayerAction
    {
        get => "PullHammer";
    }

    protected override void BindPerformed(InputAction.CallbackContext callbackContext)
    {
        performed(gunFrame.GetState());
    }

    protected override void BindCanceled(InputAction.CallbackContext callbackContext)
    {
        canceled(gunFrame.GetState());
    }

    protected override void BindStarted(InputAction.CallbackContext callbackContext)
    {
        started(gunFrame.GetState());
    }

    public delegate void HammerEvent(GunFrame.GunState gunState);
}
