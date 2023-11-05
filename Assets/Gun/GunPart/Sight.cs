using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Sight : GunPart
{
    public TargetingRange targetingRange;
    public ShootingManager shootingManager;

    private void OnEnable()
    {
        shootingManager.aimStarted += SwapToSight;
        shootingManager.aimEnded += SwapBack;
    }

    private void OnDisable()
    {
        SwapBack();
        shootingManager.aimStarted -= SwapToSight;
        shootingManager.aimEnded -= SwapBack;
    }

    private void SwapToSight()
    {
        shootingManager.owner.GetComponent<PhysicsTargeter>().targetingRange = targetingRange;
    }

    private void SwapBack()
    {
        var physicsTargeter = shootingManager.owner.GetComponent<PhysicsTargeter>();
        physicsTargeter.targetingRange = physicsTargeter.defaultTargetingRange;
    }
}
