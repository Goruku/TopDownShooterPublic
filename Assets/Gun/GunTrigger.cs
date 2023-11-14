using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class GunTrigger : InteractibleGunPart
{

    public bool doubleAction = true;
    
    protected override string DefaultPlayerAction
    {
        get => "Fire";
    }
    
    public TriggerEvent performed = state => {};
    public TriggerEvent started = state => {};
    public TriggerEvent canceled = state => {};
    public bool safety = false;
    public TriggerEventType triggerEventType;
    
    public delegate void TriggerEvent(GunFrame.GunState gunState);

    protected override void BindPerformed(InputAction.CallbackContext callbackContext)
    {
        if (safety) return;
        performed(gunFrame.GetState());
    }

    protected override void BindStarted(InputAction.CallbackContext callbackContext)
    {
        if (safety) return;
        started(gunFrame.GetState());
    }

    protected override void BindCanceled(InputAction.CallbackContext callbackContext)
    {
        if (safety) return;
        canceled(gunFrame.GetState());
    }

    [Serializable]
    public enum TriggerEventType
    {
        Performed,
        Started,
        Ended
    }

    [Serializable]
    public class TriggerHammerLink : GunFrame.GunLink<GunTrigger, GunHammer>
    {
        public TriggerEventType triggerEventType;
        
        public override void Link()
        {
            if (!gunPartActed || !gunPartReacting) return;
            
            if (gunPartActed.doubleAction)
                gunPartActed.started += gunPartReacting.CallPulledIfNotPulled;
            
            switch (triggerEventType)
            {
                case TriggerEventType.Ended:
                    gunPartActed.canceled += gunPartReacting.AttemptStrike;
                    break;
                case TriggerEventType.Performed:
                    gunPartActed.performed += gunPartReacting.AttemptStrike;
                    break;
                case TriggerEventType.Started:
                    gunPartActed.started += gunPartReacting.AttemptStrike;
                    break;
            }
        }

        public override void UnLink()
        {
            if (!gunPartActed || !gunPartReacting) return;
            gunPartActed.started -= gunPartReacting.CallPulledIfNotPulled;
            gunPartActed.canceled -= gunPartReacting.AttemptStrike;
            gunPartActed.performed -= gunPartReacting.AttemptStrike;
            gunPartActed.started -= gunPartReacting.AttemptStrike;
        }
    }
}
