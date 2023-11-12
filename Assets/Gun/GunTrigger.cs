using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class GunTrigger : InteractibleGunPart
{

    protected override string DefaultPlayerAction
    {
        get => "Fire";
    }
    
    public TriggerEvent performed = state => {};
    public TriggerEvent started = state => {};
    public TriggerEvent canceled = state => {};
    public TriggerEventType triggerEventType;
    
    public delegate void TriggerEvent(GunFrame.GunState gunState);

    protected override void BindPerformed(InputAction.CallbackContext callbackContext) => performed(gunFrame.GetState());
    protected override void BindStarted(InputAction.CallbackContext callbackContext) => started(gunFrame.GetState());
    protected override void BindCanceled(InputAction.CallbackContext callbackContext) => canceled(gunFrame.GetState());
    
    [Serializable]
    public enum TriggerEventType
    {
        Performed,
        Started,
        Ended
    }

    [Serializable]
    public struct TriggerChamberLink
    {
        public GunTrigger gunTrigger;
        public GunChamber gunChamber;
        public TriggerEventType triggerEventType;
        
        public void Link()
        {
            if (!gunTrigger || !gunChamber) return;
            switch (triggerEventType)
            {
                case TriggerEventType.Ended:
                    gunTrigger.canceled += gunChamber.ConsumeNextShot;
                    break;
                case TriggerEventType.Performed:
                    gunTrigger.performed += gunChamber.ConsumeNextShot;
                    break;
                case TriggerEventType.Started:
                    gunTrigger.started += gunChamber.ConsumeNextShot;
                    break;
            }
        }

        public void UnLink()
        {
            if (!gunTrigger || !gunChamber) return;
            switch (triggerEventType)
            {
                case TriggerEventType.Ended:
                    gunTrigger.canceled -= gunChamber.ConsumeNextShot;
                    break;
                case TriggerEventType.Performed:
                    gunTrigger.performed -= gunChamber.ConsumeNextShot;
                    break;
                case TriggerEventType.Started:
                    gunTrigger.started -= gunChamber.ConsumeNextShot;
                    break;
            }
        }
    }
}
