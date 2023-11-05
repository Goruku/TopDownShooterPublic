using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(ShootingManager))]
public class BulletFactory : MonoBehaviour
{
    private ShootingManager _shootingManager;
    public GunFrame gunFrame;
    
    public AudioSource audioSource;
    
    private void OnEnable()
    {
        _shootingManager = GetComponent<ShootingManager>();
        _shootingManager.duringShot += CreateBullets;
    }

    private void OnDisable()
    {
        _shootingManager.duringShot -= CreateBullets;
    }

    void CreateBullets(Transform fireLocation)
    {
        var shot = gunFrame.ConsumeNextShot();

        if (shot.misfire && audioSource)
        {
            audioSource.PlayOneShot(gunFrame.jam);
            return;
        }
        
        if (shot.empty)
        {
            audioSource.PlayOneShot(gunFrame.emptySound);
            return;
        }

        foreach (var bullet in shot.bullets)
        {
            var newBullet = Instantiate(bullet, fireLocation.position, fireLocation.rotation);
            newBullet.GetComponent<Rigidbody2D>().velocity = (fireLocation.rotation*Vector3.up).normalized*shot.velocity;
        }
        
        if (audioSource && gunFrame.fireSound)
            audioSource.PlayOneShot(gunFrame.fireSound);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
