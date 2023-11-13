using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(AudioSource))]
public class BulletFactory : GunPart
{
    [HideInInspector]
    public AudioSource audioSource;

    private void Reset()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void CreateBullets(GunFrame.Shot shot, Transform fireLocation)
    {
        if (shot.misfire && audioSource)
        {
            audioSource.PlayOneShot(shot.shotSound);
            //misfire firing effect?
            return;
        }
        
        if (shot.empty && audioSource)
        {
            audioSource.PlayOneShot(shot.shotSound);
            return;
        }

        foreach (var bullet in shot.bullets)
        {
            var newBullet = Instantiate(bullet, fireLocation.position, fireLocation.rotation);
            newBullet.GetComponent<Rigidbody2D>().velocity = (fireLocation.rotation*Vector3.up).normalized*shot.velocity;
        }
        
        if (audioSource && shot.shotSound)
            audioSource.PlayOneShot(shot.shotSound);
    }
}
