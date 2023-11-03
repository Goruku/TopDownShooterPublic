using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[ExecuteAlways]
public class GunMounter : MonoBehaviour
{
    public List<ShootingManager> _currentManagers = new List<ShootingManager>();

    private void AttemptFiringAll()
    {
        foreach (var shootingManager in _currentManagers)
        {
            shootingManager.AttemptFiring();
        }
    }
    
    private void OnTransformChildrenChanged()
    {
        var shootingManagers = transform.GetComponentsInChildren<ShootingManager>();
        foreach (var shootingManager in shootingManagers)
        {
            if (!_currentManagers.Contains(shootingManager))
            {
                _currentManagers.Add(shootingManager);
                shootingManager.playerOwned = true;
            }
        }

        foreach (var shootingManager in _currentManagers.ToList())
        {
            if (!shootingManagers.Contains(shootingManager))
            {
                _currentManagers.Remove(shootingManager);
                shootingManager.playerOwned = false;
            }
        }
    }
}
