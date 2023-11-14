using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunRepeatingChamber : GunChamber, ISerializationCallbackReceiver
{
    public List<ChamberHammerLink> chamberHammerLinks;

    public void OnBeforeSerialize()
    {
        
    }

    public void OnAfterDeserialize()
    {
        GunFrame.ManualLinkGunParts<GunChamber.ChamberHammerLink, GunChamber, GunHammer>(chamberHammerLinks);
    }
}
