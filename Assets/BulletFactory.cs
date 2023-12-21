using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class BulletFactory : GunPart
{
    [SerializeField]
    private PoolManagerProvider poolManagerProvider;
    [SerializeField]
    private PoolablePrefab.PoolId poolId;
    
    public void CreateBullets(GunFrame.Shot shot, Transform fireLocation)
    {
        if (shot.misfire)
        {
            //misfire firing effect?
            return;
        }
        
        if (shot.empty)
        {
            return;
        }

        foreach (var bullet in shot.bullets)
        {
            GameObject newBullet;
            if (poolManagerProvider)
            {
                newBullet = poolManagerProvider.PoolManager.FetchByPoolId(poolId);
                if (!newBullet)
                {
                    return;
                }
                newBullet.transform.position = fireLocation.position;
                newBullet.transform.rotation = fireLocation.rotation;
            }
            else
            {
                newBullet = Instantiate(bullet, fireLocation.position, fireLocation.rotation);
            }
            newBullet.GetComponent<Rigidbody2D>().velocity = (fireLocation.rotation*Vector3.up).normalized*shot.velocity;
        }

    }
}
