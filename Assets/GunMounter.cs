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
    public ShootingManager shootingManager;

    private void AttemptFiring()
    {
        shootingManager.AttemptFiring();
    }

    public void SwapGun(ShootingManager otherShooterManager)
    {
        var formerParent = otherShooterManager.transform.parent;
        if (!formerParent)
        {
            otherShooterManager.transform.parent = shootingManager.transform.parent;
        }
        else
        {
            otherShooterManager.transform.SetParent(shootingManager.transform.parent, true);
        }
        shootingManager.transform.SetParent(formerParent, true);
        
        shootingManager.playerOwned = false;
        shootingManager = otherShooterManager;
        shootingManager.playerOwned = true;
        shootingManager.transform.localPosition = new Vector3();
        shootingManager.transform.rotation = new Quaternion();
    }

    private void OnTransformChildrenChanged()
    {
        var shootingManagers = transform.GetComponentsInChildren<ShootingManager>();
        if (shootingManagers.Length <= 0) return;
        shootingManager = shootingManagers[0];
        shootingManager.playerOwned = true;
    }
}
