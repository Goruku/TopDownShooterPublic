using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class AlternatingTrigger : GunTrigger, ISerializationCallbackReceiver
{
    public int currentActiveTrigger = 0;
    public List<TriggerHammerLink> triggerHammerLinks = new ();

    public void ChangeTrigger()
    {
        if (currentActiveTrigger >= triggerHammerLinks.Count)
        {
            currentActiveTrigger = 0;
        }
        else
        {
            triggerHammerLinks[currentActiveTrigger].UnLink();
            triggerHammerLinks[currentActiveTrigger].active = false;
        }
        currentActiveTrigger = ++currentActiveTrigger >= triggerHammerLinks.Count ? 0 : currentActiveTrigger;
        triggerHammerLinks[currentActiveTrigger].active = true;
    }

    protected override void BindPerformed(InputAction.CallbackContext callbackContext)
    {
        base.BindPerformed(callbackContext);
        ChangeTrigger();
        GunFrame.ManualLinkGunParts<TriggerHammerLink, GunTrigger, GunHammer>(triggerHammerLinks);
    }

    public new void OnAfterDeserialize()
    {
        base.OnAfterDeserialize();
        
        
        GunFrame.ManualLinkGunParts<TriggerHammerLink, GunTrigger, GunHammer>(triggerHammerLinks);
    }
}
