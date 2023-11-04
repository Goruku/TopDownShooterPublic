using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

[RequireComponent(typeof(ShootingManager))]
public class RecoilManager : MonoBehaviour
{
    public MouseRecoilPattern mouseRecoilPattern;
    public PhysicsRecoilPattern physicsRecoilPattern;
    private ShootingManager _shootingManager;
    public Rigidbody2D physicsObject;

    private void Awake()
    {
        _shootingManager = GetComponent<ShootingManager>();
        
    }

    private void OnEnable()
    {
        _shootingManager.afterShot += ApplyRecoil;
    }

    private void OnDisable()
    {
        _shootingManager.afterShot -= ApplyRecoil;
    }

    private void OnTransformParentChanged()
    {
        var owner = _shootingManager.owner;
        if (owner)
        {
            physicsObject = owner.GetComponent<Rigidbody2D>();
        }
        physicsObject = GetComponent<Rigidbody2D>();
    }

    public void ApplyRecoil(Transform pointerLocation)
    {
        if (mouseRecoilPattern)
        {
            var recoil = mouseRecoilPattern.GetNext();
            Mouse.current.WarpCursorPosition(Mouse.current.position.value + recoil);
        }

        if (physicsRecoilPattern)
        {
            var recoil = physicsRecoilPattern.GetNext();
            physicsObject.AddForce(physicsObject.transform.rotation*recoil.force);
            physicsObject.AddTorque(recoil.torque);
        }
    }
}
