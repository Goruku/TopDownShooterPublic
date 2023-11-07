using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Sight : GunPart
{
    public TargetingRange targetingRange;

    private new void OnEnable()
    {
        base.OnEnable();
        gunFrame.shootingManager.aimStarted += SwapToSight;
        gunFrame.shootingManager.aimEnded += SwapBack;
    }

    private new void OnDisable()
    {
        base.OnDisable();
        SwapBack();
        gunFrame.shootingManager.aimStarted -= SwapToSight;
        gunFrame.shootingManager.aimEnded -= SwapBack;
    }

    private void SwapToSight()
    {
        if (gunFrame.shootingManager.owner)
            gunFrame.shootingManager.owner.GetComponent<PhysicsTargeter>().targetingRange = targetingRange;
    }

    private void SwapBack()
    {
        if (gunFrame.shootingManager.owner)
        {
            var physicsTargeter = gunFrame.shootingManager.owner.GetComponent<PhysicsTargeter>();
            physicsTargeter.targetingRange = physicsTargeter.defaultTargetingRange;
        }
    }
}
