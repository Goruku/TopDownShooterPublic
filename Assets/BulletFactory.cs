using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class BulletFactory : GunPart
{
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
            var newBullet = Instantiate(bullet, fireLocation.position, fireLocation.rotation);
            newBullet.GetComponent<Rigidbody2D>().velocity = (fireLocation.rotation*Vector3.up).normalized*shot.velocity;
        }

    }
}
