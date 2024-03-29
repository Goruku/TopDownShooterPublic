using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public abstract class InteractibleGunPart : GunPart, ISerializationCallbackReceiver
{
    public bool shouldUpdateBindings = false;
    
    private string PlayerActionOverrideChecker
    {
        get => string.IsNullOrEmpty(playerActionOverride) ? DefaultPlayerAction : playerActionOverride;
    }
    
    public string playerActionOverride = "";

    protected abstract string DefaultPlayerAction
    {
        get;
    }
    
    public new void OnEnable()
    {
        base.OnEnable();
        AttemptBindAction(gunFrame.owner);
        gunFrame.ownerWillChange += AttemptUnbind;
        gunFrame.ownerChanged += AttemptBindAction;
    }

    public new void OnDisable()
    {
        base.OnDisable();
        AttemptUnbind(gunFrame.owner);
        gunFrame.ownerWillChange -= AttemptUnbind;
        gunFrame.ownerChanged -= AttemptBindAction;
    }

    private void Update()
    {
        if (!shouldUpdateBindings) return;
        AttemptRebind(gunFrame.owner);
        shouldUpdateBindings = false;
    }

    public void OnBeforeSerialize()
    {
        shouldUpdateBindings = true;
    }

    public void OnAfterDeserialize()
    {
        shouldUpdateBindings = true;
    }

    private void AttemptRebind(Actor actor)
    {
        AttemptUnbind(actor);
        AttemptBindAction(actor);
    }

    private void AttemptBindAction(Actor actor)
    {
        if (actor is Agent agent)
        {
            if (!agent.playerInputUnsafe) return;
            agent.playerInputUnsafe.actions[PlayerActionOverrideChecker].performed += BindPerformed;
            agent.playerInputUnsafe.actions[PlayerActionOverrideChecker].started += BindStarted;
            agent.playerInputUnsafe.actions[PlayerActionOverrideChecker].canceled += BindCanceled;
        }
    }

    private void AttemptUnbind(Actor actor)
    {
        if (actor is Agent agent)
        {
            if (!agent.playerInputUnsafe) return;
            agent.playerInputUnsafe.actions[PlayerActionOverrideChecker].performed -= BindPerformed;
            agent.playerInputUnsafe.actions[PlayerActionOverrideChecker].started -= BindStarted;
            agent.playerInputUnsafe.actions[PlayerActionOverrideChecker].canceled -= BindCanceled;
        }
    }

    protected abstract void BindPerformed(InputAction.CallbackContext callbackContext);
    protected abstract void BindStarted(InputAction.CallbackContext callbackContext);
    protected abstract void BindCanceled(InputAction.CallbackContext callbackContext);
}
