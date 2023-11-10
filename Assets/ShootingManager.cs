using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShootingManager : MonoBehaviour
{

    public float cooldown;
    public float lastShot;
    public Actor owner;
    
    public Transform fireLocation;

    public FiringAction duringShot = location => {};
    public FiringAction afterShot = location => {};
    
    // Start is called before the first frame update
    void Start()
    {
        lastShot = Time.time;
    }

    private void OnTransformParentChanged()
    {
        OnDisable();
        Actor.BindToClosestActor(transform, out owner);
        OnEnable();
    }

    private void OnEnable()
    {
        if (owner is Agent agent)
        {
            agent.playerInput.actions["Fire"].performed += AttemptFiring;
        }
    }

    private void OnDisable()
    {
        if (owner is Agent agent)
        {
            agent.playerInput.actions["Fire"].performed -= AttemptFiring;
        }
    }

    public void AttemptFiring(InputAction.CallbackContext callbackContext)
    {
        if (lastShot + cooldown <= Time.time)
            Fire();
    }

    void Fire()
    {
        lastShot = Time.fixedTime;
        duringShot(fireLocation);
        afterShot(fireLocation);
    }

}

public delegate void FiringAction(Transform pointerLocation);
