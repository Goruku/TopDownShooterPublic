using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Sight : GunPart
{
    public TargetingRange targetingRange;

    private new void OnEnable()
    {
        base.OnEnable();
        gunFrame.ownerWillChange += AttemptUnbindAim;
        gunFrame.ownerChanged += AttemptBindAim;
    }

    private new void OnDisable()
    {
        base.OnDisable();
        SwapBack();
        gunFrame.ownerWillChange -= AttemptUnbindAim;
        gunFrame.ownerChanged -= AttemptBindAim;
    }

    private void AttemptBindAim(Actor actor)
    {
        if (actor is Agent agent)
        {
            agent.playerInput.actions["Aim"].started += StartAim;
            agent.playerInput.actions["Aim"].canceled += EndAim;
        }
    }

    private void AttemptUnbindAim(Actor actor)
    {
        if (actor is Agent agent)
        {
            agent.playerInput.actions["Aim"].started -= StartAim;
            agent.playerInput.actions["Aim"].canceled -= EndAim;
        }
    }

    private void SwapToSight()
    {
        if (gunFrame.owner)
            gunFrame.owner.GetComponent<PhysicsTargeter>().targetingRange = targetingRange;
    }
    
    private void SwapBack()
    {
        if (gunFrame.owner)
        {
            var physicsTargeter = gunFrame.owner.GetComponent<PhysicsTargeter>();
            physicsTargeter.targetingRange = physicsTargeter.defaultTargetingRange;
        }
    }

    private void StartAim(InputAction.CallbackContext callbackContext)
    {
        SwapToSight();
    }

    private void EndAim(InputAction.CallbackContext callbackContext)
    {
        SwapBack();
    }
}
