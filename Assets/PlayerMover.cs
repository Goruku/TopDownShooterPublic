using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMover : MonoBehaviour
{
    public Rigidbody2D rigidBody2D;
    public InputActionAsset inputActionAsset;

    public float movementSpeed;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (rigidBody2D.velocity.magnitude > movementSpeed) return;
        var movementVector = inputActionAsset.actionMaps[0]["Move"].ReadValue<Vector2>().normalized;
        if (movementVector.magnitude == 0)
        {
            rigidBody2D.velocity = new Vector2();
        }
        else
        {
            rigidBody2D.velocity = movementVector * movementSpeed;
        }
    }
}
