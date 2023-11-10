using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class GunTrigger : GunPart
{
    public TriggerEvent performed = state => {};
    public TriggerEvent started = state => {};
    public TriggerEvent canceled = state => {};
    public TriggerEventType triggerEventType;
    
    public delegate void TriggerEvent(GunFrame.GunState gunState);

    public new void OnEnable()
    {
        base.OnEnable();
        AttemptBindFire(gunFrame.owner);
        gunFrame.ownerWillChange += AttemptUnbindFire;
        gunFrame.ownerChanged += AttemptBindFire;
    }

    public new void OnDisable()
    {
        base.OnDisable();
        AttemptUnbindFire(gunFrame.owner);
        gunFrame.ownerWillChange -= AttemptUnbindFire;
        gunFrame.ownerChanged -= AttemptBindFire;
    }

    private void AttemptBindFire(Actor actor)
    {
        if (actor is Agent agent)
        {
            agent.playerInput.actions["Fire"].performed += BindPerformed;
            agent.playerInput.actions["Fire"].started += BindStarted;
            agent.playerInput.actions["Fire"].canceled += BindCanceled;
        }
    }

    private void AttemptUnbindFire(Actor actor)
    {
        if (actor is Agent agent)
        {
            agent.playerInput.actions["Fire"].performed -= BindPerformed;
            agent.playerInput.actions["Fire"].started -= BindStarted;
            agent.playerInput.actions["Fire"].canceled -= BindCanceled;
        }
    }

    private void BindPerformed(InputAction.CallbackContext callbackContext) => performed(gunFrame.GetState());
    private void BindStarted(InputAction.CallbackContext callbackContext) => started(gunFrame.GetState());
    private void BindCanceled(InputAction.CallbackContext callbackContext) => canceled(gunFrame.GetState());
    
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
