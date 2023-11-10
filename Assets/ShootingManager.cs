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
    public bool playerOwned;
    
    public Transform fireLocation;

    public FiringAction duringShot = location => {};
    public FiringAction afterShot = location => {};
    
    // Start is called before the first frame update
    void Start()
    {
        lastShot = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        AttemptFiring();
    }

    public void AttemptFiring()
    {
        if (playerOwned && Mouse.current.leftButton.isPressed)
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
        duringShot(fireLocation);
        afterShot(fireLocation);
    }

}

public delegate void FiringAction(Transform pointerLocation);
