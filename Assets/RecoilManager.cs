using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

[RequireComponent(typeof(GunBarrel))]
public class RecoilManager : MonoBehaviour
{
    public MouseRecoilPattern mouseRecoilPattern;
    public PhysicsRecoilPattern physicsRecoilPattern;
    public GunBarrel gunBarrel;
    public Rigidbody2D physicsObject;

    private void Awake()
    {
        gunBarrel = GetComponent<GunBarrel>();
    }

    private void OnEnable()
    {
        var owner = gunBarrel.gunFrame.owner;
        if (owner)
        {
            physicsObject = owner.GetComponent<Rigidbody2D>();
        }
        gunBarrel.emptied += ApplyRecoil;
    }

    private void OnDisable()
    {
        gunBarrel.emptied -= ApplyRecoil;
    }

    private void OnTransformParentChanged()
    {
        OnDisable();
        OnEnable();
    }
    
    public void ApplyRecoil(GunFrame.Shot shot, Transform pointerLocation)
    {
        if (mouseRecoilPattern)
        {
            var recoil = mouseRecoilPattern.GetNext(shot.randomness);
            Mouse.current.WarpCursorPosition(Mouse.current.position.value + recoil);
        }

        if (physicsRecoilPattern && physicsObject)
        {
            var recoil = physicsRecoilPattern.GetNext(shot.randomness);
            physicsObject.AddForce(physicsObject.transform.rotation*recoil.force);
            physicsObject.AddTorque(recoil.torque);
        }
    }
}
