using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

public class RecoilManager : MonoBehaviour
{
    public MouseRecoilPattern mouseRecoilPattern;
    public PhysicsRecoilPattern physicsRecoilPattern;
    public Rigidbody2D physicsObject;

    public void ApplyRecoil()
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
