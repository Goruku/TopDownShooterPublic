using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

[ExecuteAlways]
public class GunMounter : MonoBehaviour
{
    public GunFrame gunFrame;
    public GunMounterEvent gunFrameChanged = (mounter, frame) => {};
    
    
    public void SwapGun(Transform target, GunFrame otherGunFrame)
    {
        var formerParent = otherGunFrame.transform.parent;
        if (gunFrame)
            gunFrame.transform.SetParent(formerParent, true);
        gunFrame = otherGunFrame;
        otherGunFrame.transform.SetParent(transform);

        gunFrame.transform.localPosition = new Vector3();
        gunFrame.transform.rotation = new Quaternion();
        gunFrameChanged(this, gunFrame);
    }

    private void OnEnable()
    {
        var gunFrames = transform.GetComponentsInChildren<GunFrame>();
        if (gunFrames.Length <= 0) return;
        gunFrame = gunFrames[0];
    }

    private void OnDisable()
    {
        
    }

    private void OnTransformChildrenChanged()
    {
        OnDisable();
        OnEnable();
    }

    public delegate void GunMounterEvent(GunMounter gunMounter, GunFrame gunFrame);
}
