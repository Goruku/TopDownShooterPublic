using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WeaponGUIController : MonoBehaviour , ISerializationCallbackReceiver
{
    public List<GunMounter> attachedGunMounters = new ();
    public List<WeaponGUI> weaponGuis = new ();
    private Dictionary<int, GunFrame> gunFrameMap = new ();


    private void FixedUpdate()
    {
        var gunFrames = gunFrameMap.Values.ToList();
        for (int i = 0; i < gunFrames.Count; i++)
        {
            var gunFrame = gunFrames[i];
            if (!gunFrame) return;
            if (!weaponGuis[i]) return;

            var gunFeeder = gunFrame.GetComponentInChildren<GunFeeder>();

            weaponGuis[i].UpdateGunInfo(new GunFrameInfo()
            {
                chamberEmpty = !gunFrame.chambers[0].round,
                hammerPulled = gunFrame.hammers[0].IsPulled(),
                defaultMagazineSize = gunFeeder ? gunFeeder.defaultMagazine.Count : 0,
                currentMagazineSize = gunFeeder ? gunFeeder.ammunition.Count : 0
            });
        }
    }

    void BindNewGun(GunMounter gunMounter, GunFrame gunFrame)
    {
        gunFrameMap[gunMounter.GetInstanceID()] = gunFrame;
    }

    public void OnBeforeSerialize()
    {
        foreach (var attachedGunMounter in attachedGunMounters)
        {
            attachedGunMounter.gunFrameChanged -= BindNewGun;
        }
        gunFrameMap.Clear();
        FetchAllAttachedGuns();
    }

    public void OnAfterDeserialize()
    {
        foreach (var attachedGunMounter in attachedGunMounters)
        {
            attachedGunMounter.gunFrameChanged += BindNewGun;
        }
        gunFrameMap.Clear();
        FetchAllAttachedGuns();
    }

    public void FetchAllAttachedGuns()
    {
        foreach (var attachedGunMounter in attachedGunMounters)
        {
            gunFrameMap[attachedGunMounter.GetInstanceID()] = attachedGunMounter.gunFrame;
        }
    }

    [Serializable]
    public struct GunFrameInfo
    {
        public bool chamberEmpty;
        public bool hammerPulled;
        public int defaultMagazineSize;
        public int currentMagazineSize;
    }
}
