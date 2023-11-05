using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(ShootingManager))]
public class BulletFactory : MonoBehaviour
{
    private ShootingManager _shootingManager;
    public GunFrame gunFrame;
    
    public AudioSource audioSource;
    
    private void OnEnable()
    {
        _shootingManager = GetComponent<ShootingManager>();
        _shootingManager.shotTrigger += CreateBullets;
    }

    private void OnDisable()
    {
        _shootingManager.shotTrigger -= CreateBullets;
    }

    void CreateBullets(Transform fireLocation)
    {
        var shot = gunFrame.ConsumeNextShot();

        if (shot.misfire && audioSource)
        {
            audioSource.PlayOneShot(gunFrame.jam);
            //misfire firing effect?
            return;
        }
        
        if (shot.empty && audioSource)
        {
            audioSource.PlayOneShot(gunFrame.emptySound);
            return;
        }

        foreach (var bullet in shot.bullets)
        {
            var newBullet = Instantiate(bullet, fireLocation.position, fireLocation.rotation);
            newBullet.GetComponent<Rigidbody2D>().velocity = (fireLocation.rotation*Vector3.up).normalized*shot.velocity;
        }

        _shootingManager.firingEffect(fireLocation, shot);
        
        if (audioSource && gunFrame.fireSound)
            audioSource.PlayOneShot(gunFrame.fireSound);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
