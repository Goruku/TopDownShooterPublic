using System;
using System.Collections;
using System.Collections.Generic;using UnityEditor;
using UnityEngine;

[ExecuteAlways]
public class WeaponGUI : MonoBehaviour
{
    public GUIChamberInfo guiChamberInfo;
    public GUIFeederInfo guiFeederInfo;
    public GUIHammerInfo hammerInfo;

    public void UpdateGunInfo(WeaponGUIController.GunFrameInfo gunFrameInfo)
    {
        hammerInfo.UpdateHammerState(gunFrameInfo.hammerPulled);
        guiFeederInfo.UpdateFeederText(gunFrameInfo.currentMagazineSize, gunFrameInfo.defaultMagazineSize);
        guiChamberInfo.UpdateChamberState(gunFrameInfo.chamberEmpty);
    }
}
