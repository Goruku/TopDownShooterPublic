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
        var owner = _shootingManager.owner;
        if (owner)
        {
            physicsObject = owner.GetComponent<Rigidbody2D>();
        }
        _shootingManager.firingEffect += ApplyRecoil;
    }

    private void OnDisable()
    {
        _shootingManager.firingEffect -= ApplyRecoil;
    }

    private void OnTransformParentChanged()
    {
        OnDisable();
        OnEnable();
    }

    public void ApplyRecoil(Transform pointerLocation, GunFrame.Shot shot)
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
