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

    public FiringAction shotTrigger = location => {};
    public FiringEffect firingEffect = (location, shot) => {};
    public AimAction aimStarted = () => {};
    public AimAction aimEnded = () => {};
    
    // Start is called before the first frame update
    void Start()
    {
        lastShot = Time.time;
    }

    private void OnTransformParentChanged()
    {
        OnDisable();
        Entity.BindToClosest(transform, out owner);
        OnEnable();
    }

    private void OnEnable()
    {
        if (owner is Agent agent)
        {
            agent.playerInput.actions["Fire"].performed += AttemptFiring;
            agent.playerInput.actions["Aim"].started += AimStart;
            agent.playerInput.actions["Aim"].canceled += AimEnd;
        }
    }

    private void OnDisable()
    {
        if (owner is Agent agent)
        {
            agent.playerInput.actions["Fire"].performed -= AttemptFiring;
            agent.playerInput.actions["Aim"].started -= AimStart;
            agent.playerInput.actions["Aim"].canceled -= AimEnd;
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
        shotTrigger(fireLocation);
    }

    void AimStart(InputAction.CallbackContext callbackContext)
    {
        aimStarted();
    }

    void AimEnd(InputAction.CallbackContext callbackContext)
    {
        aimEnded();
    }

}

public delegate void FiringAction(Transform pointerLocation);

public delegate void FiringEffect(Transform pointerLocation, GunFrame.Shot shot);

public delegate void AimAction();
