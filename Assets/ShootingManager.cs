using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShootingManager : MonoBehaviour
{

    public float cooldown;
    public float lastShot;
    public bool ready = false;
    public Transform pointerLocation;
    public GameObject pointerPrefab;

    public float bulletSpeed;

    public GameObject bulletPrefab;
    public Transform fireLocation;
    public AudioSource audioSource;

    public RecoilManager recoilManager;
    
    // Start is called before the first frame update
    void Start()
    {
        lastShot = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (Mouse.current.leftButton.isPressed)
        {
            if (lastShot + cooldown <= Time.time)
                ready = true;
        }
    }

    private void FixedUpdate()
    {
        if (!ready) return;
        Fire();
    }

    void Fire()
    {
        lastShot = Time.fixedTime;
        ready = false;
        if (pointerLocation)
        {
            Instantiate(pointerPrefab, pointerLocation.position, pointerLocation.rotation);
        }
        var newBullet = Instantiate(bulletPrefab, fireLocation.position, fireLocation.rotation);
        newBullet.GetComponent<Rigidbody2D>().velocity = (fireLocation.rotation*Vector3.up).normalized*bulletSpeed;
        audioSource.PlayOneShot(audioSource.clip);
        recoilManager.ApplyRecoil();
    }

}
