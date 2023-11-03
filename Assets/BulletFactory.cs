using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(ShootingManager))]
public class BulletFactory : MonoBehaviour
{
    private ShootingManager _shootingManager;
    public GameObject bulletPrefab;
    public float bulletSpeed;
    
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
        var newBullet = Instantiate(bulletPrefab, fireLocation.position, fireLocation.rotation);
        newBullet.GetComponent<Rigidbody2D>().velocity = (fireLocation.rotation*Vector3.up).normalized*bulletSpeed;
        
        if (audioSource)
            audioSource.PlayOneShot(audioSource.clip);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
